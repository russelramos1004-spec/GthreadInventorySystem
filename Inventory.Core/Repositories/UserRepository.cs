using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class UserRepository
    {
        private readonly SqlConnection _conn;

        public UserRepository(SqlConnection conn)
        {
            _conn = conn;
        }

        public User? Login(string username, string password)
        {
            using var cmd = new SqlCommand("SELECT * FROM Users WHERE Username=@u AND PasswordHash=@p", _conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new User
                {
                    Id = (int)r["Id"],
                    Username = r["Username"].ToString() ?? "",
                    Password = r["PasswordHash"].ToString() ?? "",
                    Role = r["Role"].ToString() ?? "",
                    CreatedAt = (DateTime)r["CreatedAt"]
                };
            }

            return null;
        }

        public List<User> GetAll()
        {
            var list = new List<User>();

            using var cmd = new SqlCommand("SELECT * FROM Users ORDER BY Id DESC", _conn);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new User
                {
                    Id = (int)r["Id"],
                    Username = r["Username"].ToString() ?? "",
                    Password = r["PasswordHash"].ToString() ?? "",
                    Role = r["Role"].ToString() ?? "",
                    CreatedAt = (DateTime)r["CreatedAt"]
                });
            }

            return list;
        }

        public bool Insert(User u)
        {
            using var cmd = new SqlCommand(
                "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@un, @pw, @role)", _conn);

            cmd.Parameters.AddWithValue("@un", u.Username);
            cmd.Parameters.AddWithValue("@pw", u.Password);
            cmd.Parameters.AddWithValue("@role", u.Role);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(User u)
        {
            using var cmd = new SqlCommand(
                "UPDATE Users SET Username=@un, PasswordHash=@pw, Role=@role WHERE Id=@id", _conn);

            cmd.Parameters.AddWithValue("@un", u.Username);
            cmd.Parameters.AddWithValue("@pw", u.Password);
            cmd.Parameters.AddWithValue("@role", u.Role);
            cmd.Parameters.AddWithValue("@id", u.Id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Users WHERE Id=@id", _conn);
            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}