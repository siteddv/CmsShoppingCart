using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        // GET /admin/products
        public async Task<IActionResult> Index()
        {
            var pagesList = await _context.Products.Include(x => x.Category).ToListAsync();

            return View(pagesList);
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
                var uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
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
