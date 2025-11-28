using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class CategoryRepository
    {
        private readonly SqlConnection _conn;

        public CategoryRepository(SqlConnection conn)
        {
            _conn = conn;
        }

       
        public List<Category> GetAll()
        {
            var list = new List<Category>();

            using var cmd = new SqlCommand("SELECT * FROM Categories ORDER BY Id DESC", _conn);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Category
                {
                    Id = (int)r["Id"],
                    Name = r["Name"].ToString() ?? "",
                    Description = r["Description"].ToString() ?? "",
                    CreatedAt = (System.DateTime)r["CreatedAt"]
                });
            }

            return list;
        }

        
        public bool Insert(Category c)
        {
            using var cmd =
                new SqlCommand("INSERT INTO Categories (Name, Description) VALUES(@n,@d)", _conn);

            cmd.Parameters.AddWithValue("@n", c.Name);
            cmd.Parameters.AddWithValue("@d", (object?)c.Description ?? DBNull.Value);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

        
        public bool Delete(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Categories WHERE Id=@id", _conn);
            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}