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
using File = System.IO.File;

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

        // POST /admin/products/create
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
                imageName = $"{Guid.NewGuid()}_{product.ImageUpload.FileName}";
                product.Image = imageName;
                var filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                    await product.ImageUpload.CopyToAsync(fs);
            }

            _context.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The product has been added!";

            return RedirectToAction("Index");
        }

        // GET /admin/products/details/id
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET /admin/products/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _context.Categories.ToListAsync();


            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryId);

            return product == null ? NotFound() : View(product);
        }

        // POST /admin/products/edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product newProduct)
        {
            var oldProduct = await _context.Products.FindAsync(id);

            var categories = await _context.Categories.ToListAsync();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", oldProduct.CategoryId);

            if (!ModelState.IsValid)
                return View(newProduct);

            oldProduct.Slug = newProduct.Name.ToLower().Replace(" ", "-");

            var slug = await _context.Products
                .Where(p => p.Id != oldProduct.Id)
                .FirstOrDefaultAsync(x => x.Slug == oldProduct.Slug);

            if (slug != null)
            {
                ModelState.AddModelError("", "The product already exists.");
                return View(newProduct);
            }

            if (newProduct.ImageUpload != null)
            {
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                if (!string.Equals(newProduct.ImageUpload.FileName, "noimage.png") && oldProduct.Image != null)
                {
                    var oldImagePath = Path.Combine(uploadDir, oldProduct.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var imageName = $"{Guid.NewGuid()}_{newProduct.ImageUpload.FileName}";
                oldProduct.Image = imageName;
                oldProduct.CategoryId = newProduct.CategoryId;
                var filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                    await newProduct.ImageUpload.CopyToAsync(fs);
            }

            _context.Update(oldProduct);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The product has been edited!";

            return RedirectToAction("Index");
        }

        // GET /admin/products/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = "The product does not exist!";
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}
