using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class ProductRepository
    {
        private readonly SqlConnection _conn;

        public ProductRepository(SqlConnection conn)
        {
            _conn = conn;
        }

        public List<Product> GetAll()
        {
            var list = new List<Product>();

            using var cmd = new SqlCommand(
                "SELECT p.Id,p.Name,p.CategoryId,p.Description,p.Price,p.Quantity,p.CreatedAt," +
                "c.Name AS CategoryName FROM Products p " +
                "LEFT JOIN Categories c ON p.CategoryId=c.Id ORDER BY p.Id DESC", _conn);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Product
                {
                    Id = (int)r["Id"],
                    Name = r["Name"].ToString() ?? "",
                    CategoryId = r["CategoryId"] == DBNull.Value ? 0 : (int)r["CategoryId"],
                    Description = r["Description"].ToString(),
                    Price = r["Price"] == DBNull.Value ? 0 : (decimal)r["Price"],
                    Quantity = r["Quantity"] == DBNull.Value ? 0 : (int)r["Quantity"],
                    CreatedAt = (DateTime)r["CreatedAt"]
                });
            }

            return list;
        }

        public bool Insert(Product p)
        {
           
            if (string.IsNullOrWhiteSpace(p.Name)) p.Name = "Unnamed Product";
            if (p.CategoryId <= 0) p.CategoryId = 1; 
            if (p.Price < 0) p.Price = 0;
            if (p.Quantity < 0) p.Quantity = 0;

            using var cmd = new SqlCommand(
                "INSERT INTO Products (Name, CategoryId, Description, Price, Quantity) " +
                "VALUES (@n, @c, @d, @pr, @q)", _conn);

            cmd.Parameters.AddWithValue("@n", p.Name);
            cmd.Parameters.AddWithValue("@c", p.CategoryId);
            cmd.Parameters.AddWithValue("@d", (object?)p.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pr", p.Price);
            cmd.Parameters.AddWithValue("@q", p.Quantity);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                
                if (ex.Number == 547)
                {
                    throw new Exception("Invalid Category ID. Please select a valid category.", ex);
                }

                throw;
            }
        }

        public bool Delete(int id)
        {
            using var cmd = new SqlCommand(
                "DELETE FROM Products WHERE Id=@id", _conn);

            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
