using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using watchstore.Data;
using watchstore.Models;

namespace watchstore.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public AdminRepository(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public void Seed()
        {
            var _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var category1 = new Categories { Name = "Luxury", Description = "Luxury watches for special events" };
            var category2 = new Categories { Name = "Sports", Description = "Sports everyday casual wear" };
            var category3 = new Categories { Name = "Accessories", Description = "Other accessories for special events or everyday wear" };

            var categories = new List<Categories>()
            {
                category1, category2, category3
            };

            var watch1 = new Watches { Name = "Rolex Submariner", Price = 3500.00M, Category = category1, PhotoURL = "https://content.rolex.com/dam/watches/family-pages/submariner/all-models/professional-watches-submariner-all-models-share.jpg", isLimited = false };
            var watch2 = new Watches { Name = "Omega Seamaster", Price = 3000.00M, Category = category1, PhotoURL = "https://k8q7r7a2.stackpathcdn.com/wp-content/uploads/2022/03/Collectors-Corner-Omega-Seamaster-Professional-300M-SMP-Bond-Watch-2531.80.00-vintage-guide-3.jpg", isLimited = false };
            var watch3 = new Watches { Name = "Casio G-SHOCK", Price = 230.00M, Category = category2, PhotoURL = "https://cdn2.chrono24.com/images/uhren/21485489-4mq8cu7b38ljt05afff02nrs-Large.jpg", isLimited = false };
            var watch4 = new Watches { Name = "Casio Timemaster", Price = 95.00M, Category = category2, PhotoURL = "https://cdn.watchcharts.com/listings/8274ce98-e169-478e-a5e9-6b9be54e2922?d=300x300", isLimited = false };
            var watch5 = new Watches { Name = "Watch Box", Price = 10.00M, Category = category3, PhotoURL = "https://image.made-in-china.com/202f0j00VotfLzdKJPkT/Wholesale-Luxury-Custom-Wood-Watch-Box-Factory-Price-Wooden-Watch-Box.jpg", isLimited = false };
            var watches = new List<Watches>()
            {
                watch1, watch2, watch3, watch4, watch5
            };

            var user1 = new IdentityUser { UserName = "test1@gmail.com", Email = "test1@gmail.com" };
            var user2 = new IdentityUser { UserName = "test2@gmail.com", Email = "test2@gmail.com" };
            var user3 = new IdentityUser { UserName = "test3@gmail.com", Email = "test3@gmail.com" };

            string userPassword = "password";

            var users = new List<IdentityUser>()
            {
                user1, user2, user3
            };

            foreach (var user in users)
            {
                _userManager.CreateAsync(user, userPassword).Wait();
            }



            var ord1 = new Order
            {
                FName = "Andrej",
                LName = "Jonoski",
                Address1 = "JNA bb",
                City = "Tetovo",
                Country = "Macedonia",
                Email = "aj22@gmail.com",
                OrderPlaced = DateTime.Now.AddDays(-2),
                PhoneNumber = "075123456",
                User = user1,
                ZipCode = "1234",
                OrderTotal = 5200.00M,
            };

            var ord2 = new Order { };
            var ord3 = new Order { };

            var orderLines = new List<OrderDetail>()
            {
                new OrderDetail { Order=ord1, Watch=watch1, Amount=1, Price=watch1.Price},
                new OrderDetail { Order=ord1, Watch=watch5, Amount=2, Price=watch5.Price},
                new OrderDetail { Order=ord1, Watch=watch3, Amount=1, Price=watch3.Price},
            };

            var orders = new List<Order>()
            {
                ord1
            };

            _context.Categories.AddRange(categories);
            _context.Watches.AddRange(watches);
            _context.Orders.AddRange(orders);
            _context.OrderDetails.AddRange(orderLines);

            _context.SaveChanges();
        }

        public void Clear()
        {

            var shoppingCartItems = _context.ShoppingCartItems.ToList();
            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);

            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }

            var orderDetails = _context.OrderDetails.ToList();
            _context.OrderDetails.RemoveRange(orderDetails);

            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);

            var watchs = _context.Watches.ToList();
            _context.Watches.RemoveRange(watchs);

            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();
        }

    }
}
