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

        // GET /admin/details/id
        public async Task<IActionResult> Details(int id)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == id);

            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
