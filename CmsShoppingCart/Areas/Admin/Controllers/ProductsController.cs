using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(CmsShoppingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET /admin/products
        public async Task<IActionResult> Index(int p = 1)
        {
            var pageSize = 6;

            var products = await _context.Products
                .Include(x => x.Category)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(products);
        }

        // GET /admin/products/create
        public async Task<IActionResult> Create() 
        {
            var categories = await _context.Categories.ToListAsync();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST /admin/pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var categories = await _context.Categories.ToListAsync();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
                return View(product);

            product.Slug = product.Name.ToLower().Replace(" ", "-");

            var slug = await _context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);

            if (slug != null)
            {
                ModelState.AddModelError("", "The product already exists.");
                return View(product);
            }

            var imageName = "noimage.png";

            if(product.ImageUpload != null)
            {
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                imageName = $"{product.ImageUpload.FileName}_{Guid.NewGuid().ToString()}";
                var filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                    await product.ImageUpload.CopyToAsync(fs);
            }

            _context.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The product has been added!";

            return RedirectToAction("Index");
        }
    }
}
