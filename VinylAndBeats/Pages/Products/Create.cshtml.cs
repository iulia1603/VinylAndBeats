using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public List<SelectListItem> Categories { get; set; } = new();

        private void LoadCategories()
        {
            Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }

        public IActionResult OnGet()
        {
            LoadCategories();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return Page();
            }

            _context.Products.Add(Product);
            _context.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}