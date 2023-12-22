using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using watchstore.Data;

namespace watchstore.Components
{
    public class CarouselMenu : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CarouselMenu(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var watches = await _context.Watches.ToListAsync();
            return View(watches);
        }
    }
}
