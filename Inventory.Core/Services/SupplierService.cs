using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class SupplierService
    {
        private readonly SupplierRepository _repo;
        public SupplierService(SupplierRepository repo) { _repo = repo; }
        public List<Supplier> GetAll() => _repo.GetAll();
        public bool Create(Supplier s) => _repo.Insert(s);
        public bool Delete(int id) => _repo.Delete(id);
    }
}
