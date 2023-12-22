using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using watchstore.Data;

namespace watchstore.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext AppDbContext;

        public ShoppingCart(ApplicationDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task AddToCartAsync(Watches watch, int amount)
        {
            var shoppingCartItem =
                    await AppDbContext.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Watch.Id == watch.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Watch = watch,
                    Amount = 1
                };

                AppDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            await AppDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveFromCartAsync(Watches watch)
        {
            var shoppingCartItem =
                    await AppDbContext.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Watch.Id == watch.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    AppDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            await AppDbContext.SaveChangesAsync();

            return localAmount;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
                return ShoppingCartItems ??
                       (ShoppingCartItems = await
                           AppDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                               .Include(s => s.Watch)
                               .ToListAsync());
        }

        public async Task ClearCartAsync()
        {
            var cartItems = AppDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            AppDbContext.ShoppingCartItems.RemoveRange(cartItems);

            await AppDbContext.SaveChangesAsync();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = AppDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Watch.Price * c.Amount).Sum();
            return total;
        }

    }
}
