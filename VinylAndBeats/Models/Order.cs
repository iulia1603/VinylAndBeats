using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal Total { get; set; }

        // legatura cu utilizatorul - adaug la pasul cu Identity.

        // relatie N-N cu Product, prin OrderItem
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}