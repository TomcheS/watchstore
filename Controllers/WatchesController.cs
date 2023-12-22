using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using watchstore.Data;
using watchstore.Models;
using watchstore.Repositories;
using watchstore.ViewModels;

namespace watchstore.Controllers
{
    public class WatchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWatchRepository _watchRepo;
        private readonly ICategoryRepository _categoryRepo;

        public WatchesController(ApplicationDbContext context, IWatchRepository watchRepo, ICategoryRepository categoryRepo)
        {
            _context = context;
            _watchRepo = watchRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: Watches
        public async Task<IActionResult> Index()
        {
            return View(await _watchRepo.GetAllIncludedAsync());
        }

        // GET: Watches
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var model = new SearchWatchesViewModel()
            {
                WatchList = await _watchRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        private async Task<List<Watches>> GetWatchesearchList(string userInput)
        {
            if (userInput == null)
            {
                return _context.Watches.Include(p => p.Category).ToList();
            }
            userInput = userInput.ToLower().Trim();

            var result = _context.Watches.Include(p => p.Category)
                .Where(p => p
                    .Name.ToLower().Contains(userInput))
                    .Select(p => p).OrderBy(p => p.Name);

            return await result.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AjaxSearchList(string searchString)
        {
            var watchList = await GetWatchesearchList(searchString);

            return PartialView(watchList);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAll([Bind("SearchText")] SearchWatchesViewModel model)
        {
            var watches = await _watchRepo.GetAllIncludedAsync();
            if (model.SearchText == null || model.SearchText == string.Empty)
            {
                model.WatchList = watches;
                return View(model);
            }

            var input = model.SearchText.Trim();
            if (input == string.Empty || input == null)
            {
                model.WatchList = watches;
                return View(model);
            }
            var searchString = input.ToLower();

            if (string.IsNullOrEmpty(searchString))
            {
                model.WatchList = watches;
            }
            else
            {
                var watchList = await _context.Watches.Include(x => x.Category).OrderBy(x => x.Name)
                     .Where(p =>
                     p.Name.ToLower().Contains(searchString)
                  || p.Price.ToString("c").ToLower().Contains(searchString)
                  || p.Category.Name.ToLower().Contains(searchString))
                    .ToListAsync();

                if (watchList.Any())
                {
                    model.WatchList = watchList;
                }
                else
                {
                    model.WatchList = new List<Watches>();
                }

            }
            return View(model);
        }

        // GET: Watches
        [AllowAnonymous]
        public async Task<IActionResult> ListCategory(string categoryName)
        {
            bool categoryExtist = _context.Categories.Any(c => c.Name == categoryName);
            if (!categoryExtist)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

            if (category == null)
            {
                return NotFound();
            }

            bool anyWatches = await _context.Watches.AnyAsync(x => x.Category == category);
            if (!anyWatches)
            {
                return NotFound($"No Watches were found in the category: {categoryName}");
            }

            var watches = _context.Watches.Where(x => x.Category == category)
                .Include(x => x.Category);

            ViewBag.CurrentCategory = category.Name;
            return View(watches);
        }

        // GET: Watches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watches = await _watchRepo.GetByIdIncludedAsync(id);

            if (watches == null)
            {
                return NotFound();
            }

            return View(watches);
        }

        // GET: Watches/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> DisplayDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watches = await _watchRepo.GetByIdIncludedAsync(id);

            

            if (watches == null)
            {
                return NotFound();
            }

            return View(watches);
        }

        // GET: Watches
        [AllowAnonymous]
        public async Task<IActionResult> SearchWatches()
        {
            var model = new SearchWatchesViewModel()
            {
                WatchList = await _watchRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchWatches([Bind("SearchText")] SearchWatchesViewModel model)
        {
            var watches = await _watchRepo.GetAllIncludedAsync();
            var search = model.SearchText.ToLower();

            if (string.IsNullOrEmpty(search))
            {
                model.WatchList = watches;
            }
            else
            {
                var watchList = _context.Watches.Include(x => x.Category).OrderBy(x => x.Name)
                    .Where(p =>
                     p.Name.ToLower().Contains(search)
                  || p.Price.ToString("c").ToLower().Contains(search)
                  || p.Category.Name.ToLower().Contains(search));

                if (watchList.Any())
                {
                    model.WatchList = watchList;
                }
                else
                {
                    model.WatchList = new List<Watches>();
                }

            }
            return View(model);
        }

        // GET: Watches/Create
        public IActionResult Create()
        {
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Watches/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,PhotoURL,isLimited,CategoriesId")] Watches watches)
        {
            if (ModelState.IsValid)
            {
                _watchRepo.Add(watches);
                await _watchRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", watches.CategoryId);
            return View(watches);
        }

        // GET: Watches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watches = await _watchRepo.GetByIdAsync(id);

            if (watches == null)
            {
                return NotFound();
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", watches.CategoryId);
            return View(watches);
        }

        // POST: Watches/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,PhotoURL,isLimited,CategoryId")] Watches watches)
        {
            if (id != watches.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _watchRepo.Update(watches);
                    await _watchRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchesExists(watches.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", watches.CategoryId);
            return View(watches);
        }

        // GET: Watches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watches = await _watchRepo.GetByIdIncludedAsync(id);

            if (watches == null)
            {
                return NotFound();
            }

            return View(watches);
        }

        // POST: Watches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var watches = await _watchRepo.GetByIdAsync(id);
            _watchRepo.Remove(watches);
            await _watchRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WatchesExists(int id)
        {
            return _watchRepo.Exists(id);
        }
    }
}
