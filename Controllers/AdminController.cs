using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using watchstore.Data;
using watchstore.Repositories;

namespace watchstore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminRepository _adminRepo;

        public AdminController(ApplicationDbContext context, IAdminRepository adminRepo)
        {
            _context = context;
            _adminRepo = adminRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Drop Database
        public IActionResult ClearDatabaseAsync()
        {
            _adminRepo.Clear();
            return RedirectToAction("Index", "Watches", null);
        }
        //Database Seeding
        public IActionResult SeedDatabaseAsync()
        {
            _adminRepo.Clear();
            _adminRepo.Seed();
            return RedirectToAction("Index", "Watches", null);
        }
    }
}
