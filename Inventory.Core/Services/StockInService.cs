using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class StockInService
    {
        private readonly StockInRepository _repo;
        public StockInService(StockInRepository repo) { _repo = repo; }
        public List<StockIn> GetAll() => _repo.GetAll();
        public bool Create(StockIn s, out string message) => _repo.Insert(s, out message);

    }
}
