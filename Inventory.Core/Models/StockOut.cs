namespace Inventory.Core.Models
{
    public class StockOut
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
