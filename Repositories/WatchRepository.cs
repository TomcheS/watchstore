using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using watchstore.Data;
using watchstore.Models;

namespace watchstore.Repositories
{
    public class WatchRepository : IWatchRepository
    {
        private readonly ApplicationDbContext _context;

        public WatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Watches> Watches => _context.Watches.Include(w => w.Category); //include here

        public IEnumerable<Watches> LimitedWatches => _context.Watches.Where(w => w.isLimited).Include(w => w.Category);

        public void Add(Watches watch)
        {
            _context.Add(watch);
        }

        public IEnumerable<Watches> GetAll()
        {
            return _context.Watches.ToList();
        }

        public async Task<IEnumerable<Watches>> GetAllAsync()
        {
            return await _context.Watches.ToListAsync();
        }

        public async Task<IEnumerable<Watches>> GetAllIncludedAsync()
        {
            return await _context.Watches.Include(w => w.Category).ToListAsync();
        }

        public IEnumerable<Watches> GetAllIncluded()
        {
            return _context.Watches.Include(w => w.Category).ToList();
        }

        public Watches GetById(int? id)
        {
            return _context.Watches.FirstOrDefault(w => w.Id == id);
        }

        public async Task<Watches> GetByIdAsync(int? id)
        {
            return await _context.Watches.FirstOrDefaultAsync(w => w.Id == id);
        }

        public Watches GetByIdIncluded(int? id)
        {
            return _context.Watches.Include(w => w.Category).FirstOrDefault(w => w.Id == id);
        }

        public async Task<Watches> GetByIdIncludedAsync(int? id)
        {
            return await _context.Watches.Include(p => p.Category).FirstOrDefaultAsync(w => w.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Watches.Any(w => w.Id == id);
        }

        public void Remove(Watches watch)
        {
            _context.Remove(watch);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Watches watch)
        {
            _context.Update(watch);
        }

    }
}
