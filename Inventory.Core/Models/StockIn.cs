namespace Inventory.Core.Models
{
    public class StockIn
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime DateIn { get; set; }
    }
}
