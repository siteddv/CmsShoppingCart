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
            var products = await _context.Products.ToListAsync();

            return View(products);
        }

        // GET /products/categorySlug/p
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            var category = await _context.Categories
                .Where(c => c.Slug.Equals(categorySlug))
                .FirstOrDefaultAsync();

            var pageSize = 6;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug.Equals("all")) {
                return await AllProductsView(pageSize);
            }

            if (category == null)
                return RedirectToAction("Index", "Products");

            var products = await _context.Products
                .Where(pr => pr.CategoryId == category.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categoryProductsCount = _context.Products
                .Where(p => p.CategoryId == category.Id)
                .Count();

            ViewBag.TotalPages = (int)Math.Ceiling((decimal) categoryProductsCount / pageSize);

            return View(products);
        }

        public async Task<IActionResult> AllProductsView(int pageSize)
        {
            var allProducts = await _context.Products.ToListAsync();

            var allProductsCount = allProducts.Count;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)allProductsCount / pageSize);
            return View(allProducts);
        }
    }
}
