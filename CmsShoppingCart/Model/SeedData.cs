using CmsShoppingCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Model
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<CmsShoppingCartContext>>();
            using var context = new CmsShoppingCartContext(dbContextOptions);

            if (context.Pages.Any())
                return;

            context.Pages.AddRange(
                new Page
                {
                    Title = "Home",
                    Slug = "home",
                    Content = "home page"
                },
                new Page
                {
                    Title = "About us",
                    Slug = "about-us",
                    Content = "about us page"
                },
                new Page
                {
                    Title = "Services",
                    Slug = "services",
                    Content = "Services page"
                },
                new Page
                {
                    Title = "Contact",
                    Slug = "contact",
                    Content = "contact page"
                }
            );

            context.SaveChanges();
        }
    }
}
