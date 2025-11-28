using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class CustomerRepository
    {
        private readonly SqlConnection _conn;

        public CustomerRepository(SqlConnection conn)
        {
            _conn = conn;
        }

       
        public List<Customer> GetAll()
        {
            var list = new List<Customer>();

            using var cmd = new SqlCommand("SELECT * FROM Customers ORDER BY Id DESC", _conn);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Customer
                {
                    Id = (int)r["Id"],
                    Name = r["Name"].ToString() ?? "",
                    Email = r["Email"].ToString() ?? "",
                    Phone = r["Phone"].ToString() ?? "",
                    Address = r["Address"].ToString() ?? "",
                    CreatedAt = (DateTime)r["CreatedAt"]
                });
            }

            return list;
        }

       
        public bool Insert(Customer c)
        {
            using var cmd = new SqlCommand(
                "INSERT INTO Customers (Name, Email, Phone, Address) VALUES(@n,@e,@p,@a)",
                _conn);

            cmd.Parameters.AddWithValue("@n", c.Name);
            cmd.Parameters.AddWithValue("@e", (object?)c.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p", (object?)c.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@a", (object?)c.Address ?? DBNull.Value);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

      
        public bool Delete(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Customers WHERE Id=@id", _conn);

            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}