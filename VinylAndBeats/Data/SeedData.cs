using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Models;

namespace VinylAndBeats.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (context.Database.IsRelational())
            context.Database.Migrate();

        string[] roleNames = ["Admin", "User"];
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var adminEmail = "admin@vinylandbeats.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!await context.Categories.AnyAsync())
        {
            var instrumente = new Category { Name = "Instrumente" };
            var viniluri = new Category { Name = "Viniluri" };
            var cduri = new Category { Name = "CD-uri" };
            var accesorii = new Category { Name = "Accesorii" };

            context.Categories.AddRange(instrumente, viniluri, cduri, accesorii);
            await context.SaveChangesAsync();

            var admin = await userManager.FindByEmailAsync(adminEmail);
            var sellerId = admin?.Id;

            context.Products.AddRange(
                new Product
                {
                    Name = "Chitară acustică Yamaha F310",
                    Description = "Chitară acustică ideală pentru începători, stare foarte bună.",
                    Price = 1200.00m,
                    Stock = 3,
                    CategoryId = instrumente.Id,
                    SellerId = sellerId
                },
                new Product
                {
                    Name = "Vinil Pink Floyd - The Wall",
                    Description = "Ediție remasterizată, dublu LP, stare excelentă.",
                    Price = 180.50m,
                    Stock = 5,
                    CategoryId = viniluri.Id,
                    SellerId = sellerId
                },
                new Product
                {
                    Name = "CD Miles Davis - Kind of Blue",
                    Description = "Album jazz clasic, disc original.",
                    Price = 75.00m,
                    Stock = 10,
                    CategoryId = cduri.Id,
                    SellerId = sellerId
                },
                new Product
                {
                    Name = "Set corzi chitară D'Addario",
                    Description = "Corzi de schimb, calibru .010, sigilate.",
                    Price = 45.00m,
                    Stock = 20,
                    CategoryId = accesorii.Id,
                    SellerId = sellerId
                }
            );

            await context.SaveChangesAsync();
        }

        if (!await context.Tags.AnyAsync())
        {
            var tags = new[]
            {
                new Tag { Name = "Rock" },
                new Tag { Name = "Jazz" },
                new Tag { Name = "Vintage" },
                new Tag { Name = "Nou" },
                new Tag { Name = "Clasic" }
            };

            context.Tags.AddRange(tags);
            await context.SaveChangesAsync();

            var firstProduct = await context.Products
                .Include(p => p.Tags)
                .FirstOrDefaultAsync();

            if (firstProduct != null)
            {
                firstProduct.Tags.Add(tags[2]); // Vintage
                firstProduct.Tags.Add(tags[0]); // Rock
                await context.SaveChangesAsync();
            }
        }
    }
}