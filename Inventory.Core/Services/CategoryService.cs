using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repo;
        public CategoryService(CategoryRepository repo) { _repo = repo; }
        public List<Category> GetAll() => _repo.GetAll();
        public bool Create(Category c) => _repo.Insert(c);
        public bool Delete(int id) => _repo.Delete(id);
    }
}
