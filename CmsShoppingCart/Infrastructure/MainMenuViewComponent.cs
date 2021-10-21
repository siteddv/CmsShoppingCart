using CmsShoppingCart.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly CmsShoppingCartContext _context;

        public MainMenuViewComponent(CmsShoppingCartContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();

            return View(pages);
        }

        private async Task<List<Page>> GetPagesAsync()
        {
            var pages = await _context.Pages.Where(p => !p.Slug.Equals("home")).ToListAsync();

            return pages;
        }
    }
} 
