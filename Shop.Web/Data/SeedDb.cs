using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Web.Data.Entities;

namespace Shop.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext context;
        private readonly UserManager<User> _userManager;
        private Random random;

        public SeedDb(DataContext context, UserManager<User> userManager)
        {
            this.context = context;
            _userManager = userManager;
            random = new Random();
        }

        public async Task SeedAsync()
        {
            await context.Database.EnsureCreatedAsync();

            var user = await _userManager.FindByEmailAsync("salas.john@hotmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "John",
                    LastName = "Salas",
                    Email = "salas.john@hotmail.com",
                    UserName = "salas.john@hotmail.com",
                    PhoneNumber = "987575442"
                };

                var result = await _userManager.CreateAsync(user, "123456");

                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create the user in seeder.");
            }

            if (!context.Products.Any())
            {
                AddProduct("iPhone X", user);
                AddProduct("Magic Mouse", user);
                AddProduct("iPad 2019", user);
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            context.Products.Add(new Product
            {
                Name = name,
                Price = random.Next(1000),
                IsAvailable = true,
                Stock = random.Next(100),
                User = user
            });
        }
    }
}