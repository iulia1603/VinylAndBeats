using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        // relatie 1-N: o categorie are mai multe produse
        public List<Product> Products { get; set; } = new();
    }
}