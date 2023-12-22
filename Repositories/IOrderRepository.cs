using System.Threading.Tasks;
using watchstore.Models;

namespace watchstore.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);

    }
}
