using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class StockOutService
    {
        private readonly StockOutRepository _repo;
        public StockOutService(StockOutRepository repo) { _repo = repo; }
        public List<StockOut> GetAll() => _repo.GetAll();
        public bool Create(StockOut s, out string message)
        {
            return _repo.Insert(s, out message);
        }

    }
}
