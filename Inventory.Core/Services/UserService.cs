using Inventory.Core.Models;
using Inventory.Core.Repositories;

namespace Inventory.Core.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;
        public UserService(UserRepository repo) { _repo = repo; }
        public User? Login(string u, string p) => _repo.Login(u,p);
        public List<User> GetAll() => _repo.GetAll();
        public bool Create(User user) => _repo.Insert(user);
        public bool Update(User user) => _repo.Update(user);
        public bool Delete(int id) => _repo.Delete(id);
    }
}
