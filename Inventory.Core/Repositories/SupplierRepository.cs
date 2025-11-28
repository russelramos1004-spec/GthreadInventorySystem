using Inventory.Core.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Core.Repositories
{
    public class SupplierRepository
    {
        private readonly SqlConnection _conn;

        public SupplierRepository(SqlConnection conn)
        {
            _conn = conn;
        }

        public List<Supplier> GetAll()
        {
            var list = new List<Supplier>();

            using var cmd = new SqlCommand(
                "SELECT Id, Name, Phone, Email FROM Suppliers ORDER BY Id DESC",
                _conn);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Supplier
                {
                    Id = (int)r["Id"],
                    Name = r["Name"].ToString() ?? "",
                    Phone = r["Phone"].ToString() ?? "",
                    Email = r["Email"].ToString() ?? ""
                });
            }

            return list;
        }

        public bool Insert(Supplier s)
        {
            using var cmd = new SqlCommand(
                "INSERT INTO Suppliers (Name, Email, Phone) VALUES(@n, @e, @p)",
                _conn);

            cmd.Parameters.AddWithValue("@n", s.Name);
            cmd.Parameters.AddWithValue("@e", s.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p", s.Phone ?? (object)DBNull.Value);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }

       
        public bool Delete(int id)
        {
            using var cmd = new SqlCommand(
                "DELETE FROM Suppliers WHERE Id=@id",
                _conn);

            cmd.Parameters.AddWithValue("@id", id);

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}