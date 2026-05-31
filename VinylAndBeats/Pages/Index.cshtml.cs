using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();

        public void OnGet()
        {
            Products = _context.Products
                .Include(p => p.Category)
                .ToList();
        }
    }
}