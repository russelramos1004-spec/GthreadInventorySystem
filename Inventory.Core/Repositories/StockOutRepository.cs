using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class StockOutRepository
    {
        private readonly SqlConnection _conn;

        public StockOutRepository(SqlConnection conn)
        {
            _conn = conn;
        }

        
        public List<StockOut> GetAll()
        {
            var list = new List<StockOut>();

            using var cmd = new SqlCommand(
                "SELECT Id, ProductId, CustomerId, Quantity, DateOut, TotalAmount FROM StockOut ORDER BY Id DESC",
                _conn
            );

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new StockOut
                {
                    Id = (int)r["Id"],
                    ProductId = (int)r["ProductId"],
                    CustomerId = (int)r["CustomerId"],
                    Quantity = (int)r["Quantity"],
                    TotalAmount = r["TotalAmount"] == DBNull.Value ? 0 : (decimal)r["TotalAmount"],
                    Date = (DateTime)r["DateOut"]
                });
            }

            return list;
        }

        
        public bool Insert(StockOut s, out string message)
        {
            message = "";

            
            using var checkProduct = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Id = @p", _conn);
            checkProduct.Parameters.AddWithValue("@p", s.ProductId);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            var productExists = (int)checkProduct.ExecuteScalar() > 0;
            if (!productExists)
            {
                message = "Invalid ProductId: Product does not exist.";
                return false;
            }

            
            using var checkCustomer = new SqlCommand("SELECT COUNT(*) FROM Customers WHERE Id = @c", _conn);
            checkCustomer.Parameters.AddWithValue("@c", s.CustomerId);
            var customerExists = (int)checkCustomer.ExecuteScalar() > 0;
            if (!customerExists)
            {
                message = "Invalid CustomerId: Customer does not exist.";
                return false;
            }

            
            using var cmd = new SqlCommand(
                "INSERT INTO StockOut (ProductId, CustomerId, Quantity, TotalAmount, DateOut) VALUES(@p,@c,@q,@t,@d)",
                _conn
            );

            cmd.Parameters.AddWithValue("@p", s.ProductId);
            cmd.Parameters.AddWithValue("@c", s.CustomerId);
            cmd.Parameters.AddWithValue("@q", s.Quantity);
            cmd.Parameters.AddWithValue("@t", s.TotalAmount);
            cmd.Parameters.AddWithValue("@d", s.Date);

            var rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                message = "StockOut added successfully.";
                return true;
            }
            else
            {
                message = "Failed to add StockOut.";
                return false;
            }
        }

       
        public bool Delete(int id, out string message)
        {
            message = "";
            using var cmd = new SqlCommand("DELETE FROM StockOut WHERE Id = @id", _conn);
            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

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

        
        public StockOut? GetById(int id)
        {
            using var cmd = new SqlCommand("SELECT * FROM StockOut WHERE Id = @id", _conn);
            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new StockOut
                {
                    Id = (int)r["Id"],
                    ProductId = (int)r["ProductId"],
                    CustomerId = (int)r["CustomerId"],
                    Quantity = (int)r["Quantity"],
                    TotalAmount = r["TotalAmount"] == DBNull.Value ? 0 : (decimal)r["TotalAmount"],
                    Date = (DateTime)r["DateOut"]
                };
            }

            return null;
        }
    }
}
