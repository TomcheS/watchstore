using Microsoft.AspNetCore.Mvc;
using watchstore.Repositories;
using watchstore.Models;
using watchstore.ViewModels;
using watchstore.Data;
using System.Threading.Tasks;

namespace watchstore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IWatchRepository _watchRepository;
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCart _shoppingCart;
        
        public ShoppingCartController(IWatchRepository watchRepository,
            ShoppingCart shoppingCart, ApplicationDbContext context)
        {
            _watchRepository = watchRepository;
            _shoppingCart = shoppingCart;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _shoppingCart.GetShoppingCartItemsAsync();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> AddToShoppingCart(int watchId)
        {
            var selectedWatch = await _watchRepository.GetByIdAsync(watchId);

            if (selectedWatch != null)
            {
                await _shoppingCart.AddToCartAsync(selectedWatch, 1);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int watchId)
        {
            var selectedwatch = await _watchRepository.GetByIdAsync(watchId);

            if (selectedwatch != null)
            {
                await _shoppingCart.RemoveFromCartAsync(selectedwatch);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCart.ClearCartAsync();

            return RedirectToAction("Index");
        }

    }
}