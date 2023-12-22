using System.Collections.Generic;
using System.Threading.Tasks;
using watchstore.Models;

namespace watchstore.Repositories
{
    public interface IWatchRepository
    {
        IEnumerable<Watches> Watches { get; }
        IEnumerable<Watches> LimitedWatches { get; }

        Watches GetById(int? id);
        Task<Watches> GetByIdAsync(int? id);

        Watches GetByIdIncluded(int? id);
        Task<Watches> GetByIdIncludedAsync(int? id);

        bool Exists(int id);

        IEnumerable<Watches> GetAll();
        Task<IEnumerable<Watches>> GetAllAsync();

        IEnumerable<Watches> GetAllIncluded();
        Task<IEnumerable<Watches>> GetAllIncludedAsync();

        void Add(Watches watch);
        void Update(Watches watch);
        void Remove(Watches watch);

        void SaveChanges();
        Task SaveChangesAsync();

    }
}
