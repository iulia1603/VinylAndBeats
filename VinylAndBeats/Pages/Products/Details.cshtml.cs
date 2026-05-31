using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Product? Product { get; set; }

        public IActionResult OnGet(int id)
        {
            Product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (Product == null)
                return NotFound();

            return Page();
        }
    }
}