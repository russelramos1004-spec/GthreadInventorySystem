using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class StockInRepository
    {
        private readonly string _connString;
        private readonly ProductRepository _productRepo;
        private readonly SupplierRepository _supplierRepo;

        public StockInRepository(string connString, ProductRepository productRepo, SupplierRepository supplierRepo)
        {
            _connString = connString;
            _productRepo = productRepo;
            _supplierRepo = supplierRepo;
        }

        private SqlConnection GetConnection() => new SqlConnection(_connString);

        public List<StockIn> GetAll()
        {
            var list = new List<StockIn>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("SELECT * FROM StockIn ORDER BY Id DESC", conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new StockIn
                {
                    Id = (int)reader["Id"],
                    ProductId = (int)reader["ProductId"],
                    SupplierId = (int)reader["SupplierId"],
                    Quantity = (int)reader["Quantity"],
                    TotalCost = (decimal)reader["TotalCost"],
                    DateIn = (DateTime)reader["DateIn"]
                });
            }

            return list;
        }

        
        public bool Insert(StockIn s, out string message)
        {
            message = "";

            
            if (s.ProductId <= 0 || !_productRepo.GetAll().Any(p => p.Id == s.ProductId))
            {
                message = "Invalid Product. Please select a valid product.";
                return false;
            }

           
            if (s.SupplierId <= 0 || !_supplierRepo.GetAll().Any(sup => sup.Id == s.SupplierId))
            {
                message = "Invalid Supplier. Please select a valid supplier.";
                return false;
            }

            
            if (s.Quantity <= 0)
            {
                message = "Quantity must be greater than zero.";
                return false;
            }

            if (s.TotalCost < 0)
            {
                message = "Total cost cannot be negative.";
                return false;
            }

            
            using var conn = GetConnection();
            using var cmd = new SqlCommand(
                "INSERT INTO StockIn (ProductId, SupplierId, Quantity, TotalCost, DateIn) " +
                "VALUES (@p, @s, @q, @t, @d)", conn);

            cmd.Parameters.AddWithValue("@p", s.ProductId);
            cmd.Parameters.AddWithValue("@s", s.SupplierId);
            cmd.Parameters.AddWithValue("@q", s.Quantity);
            cmd.Parameters.AddWithValue("@t", s.TotalCost);
            cmd.Parameters.AddWithValue("@d", s.DateIn == default ? DateTime.Now : s.DateIn);

            conn.Open();
            var rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                message = "StockIn added successfully.";
                return true;
            }
            else
            {
                message = "Failed to add StockIn.";
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            message = "";
            using var conn = GetConnection();
            using var cmd = new SqlCommand("DELETE FROM StockIn WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            var rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                message = "Deleted successfully.";
                return true;
            }
            else
            {
                message = "Record not found.";
                return false;
            }
        }

        public StockIn? GetById(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("SELECT * FROM StockIn WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new StockIn
                {
                    Id = (int)reader["Id"],
                    ProductId = (int)reader["ProductId"],
                    SupplierId = (int)reader["SupplierId"],
                    Quantity = (int)reader["Quantity"],
                    TotalCost = (decimal)reader["TotalCost"],
                    DateIn = (DateTime)reader["DateIn"]
                };
            }
            return null;
        }
    }
}
