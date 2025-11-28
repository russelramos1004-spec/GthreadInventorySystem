using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repo;

        public ProductService(ProductRepository repo)
        {
            _repo = repo;
        }

        public List<Product> GetAll() => _repo.GetAll();

        public bool Create(Product p) => _repo.Insert(p);

        public bool Delete(int id) => _repo.Delete(id);
    }
}
