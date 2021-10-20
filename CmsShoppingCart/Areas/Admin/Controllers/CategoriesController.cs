using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public CategoriesController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        // GET /admin/categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            return View(categories);
        }

        // GET /admin/categories/create
        public IActionResult Create() => View();

        // POST /admin/categories/create
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            category.Slug = category.Name.ToLower().Replace(" ", "-");

            var slug = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);

            if (slug != null)
            {
                ModelState.AddModelError("", "The category already exists.");
                return View(category);
            }

            _context.Add(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The category has been added!";

            return RedirectToAction("Index");
        }

        // GET /admin/categories/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST /admin/categories/create
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            category.Slug = category.Name.ToLower().Replace(" ", "-");

            var slug = await _context.Categories.FirstOrDefaultAsync(x => x.Id != category.Id && x.Slug == category.Slug);

            if (slug != null)
            {
                ModelState.AddModelError("", "The category already exists.");
                return View(category);
            }

            _context.Update(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The category has been edited!";

            return RedirectToAction("Edit", new { Id = category.Id });
        }

        // GET /admin/categories/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            var categoryProducts = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();

            foreach(var product in categoryProducts)
            {
                product.Category = null;
                product.CategoryId = null;

                _context.Update(product);
                await _context.SaveChangesAsync();
            }

            if (category == null)
            {
                TempData["Error"] = "The category does not exist!";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The category has been deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}
