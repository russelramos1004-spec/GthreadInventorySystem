using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _repo;
        public CustomerService(CustomerRepository repo) { _repo = repo; }
        public List<Customer> GetAll() => _repo.GetAll();
        public bool Create(Customer c) => _repo.Insert(c);
        public bool Delete(int id) => _repo.Delete(id);
    }
}
