namespace VinylAndBeats.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // legatura catre Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // legatura catre Product
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}