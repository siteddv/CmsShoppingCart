using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public PagesController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        // GET /admin/pages
        public async Task<IActionResult> Index()
        {
            var pages = from p in _context.Pages orderby p.Sorting select p;

            var pagesList = await pages.ToListAsync();

            return View(pagesList);
        }

        // GET /admin/pages/details/id
        public async Task<IActionResult> Details(int id)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == id);

            if (page == null)
                return NotFound();

            return View(page);
        }

        // GET /admin/pages/create
        public IActionResult Create(int id) => View();

        // POST /admin/pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Page page)
        {
            if (!ModelState.IsValid)
                return View(page);

            page.Slug = page.Title.ToLower().Replace(" ", "-");
            page.Sorting = 100;

            var slug = await _context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
            
            if(slug != null)
            {
                ModelState.AddModelError("", "The title already exists.");
                return View(page);
            }

            _context.Add(page);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
