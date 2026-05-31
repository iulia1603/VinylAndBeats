using Microsoft.AspNetCore.Identity;

namespace VinylAndBeats.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();

    public List<Order> Orders { get; set; } = new();
}