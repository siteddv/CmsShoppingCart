using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public ProductsController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        // GET /products/p
        public async Task<IActionResult> Index(int p = 1)
        {
            var pageSize = 6;

            var products = await _context.Products
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(products);
        }

        // GET /products/category/p
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            var category = await _context.Categories
                .Where(c => c.Slug.Equals(categorySlug))
                .FirstOrDefaultAsync();

            if (category == null)
                RedirectToAction("Index");

            var pageSize = 6;

            var pro = await _context.Products.ToListAsync();

            var products = await _context.Products
                .Where(pr => pr.CategoryId == category.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;

            var categoryProductsCount = _context.Products
                .Where(p => p.CategoryId == category.Id)
                .Count();

            ViewBag.TotalPages = (int)Math.Ceiling((decimal) categoryProductsCount / pageSize);

            return View(products);
        }
    }
}
