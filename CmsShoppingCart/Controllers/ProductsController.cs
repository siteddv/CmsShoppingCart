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
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            return View(products);
        }

        // GET /products/categorySlug/p
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            var category = await _context.Categories
                .Where(c => c.Slug.Equals(categorySlug))
                .FirstOrDefaultAsync();

            if (category == null)
                return RedirectToAction("Index");

            var pageSize = 6;

            var pro = await _context.Products.ToListAsync();

            var products = await _context.Products
                .Where(pr => pr.CategoryId == category.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            var categoryProductsCount = _context.Products
                .Where(p => p.CategoryId == category.Id)
                .Count();

            ViewBag.TotalPages = (int)Math.Ceiling((decimal) categoryProductsCount / pageSize);

            return View(products);
        }
    }
}
