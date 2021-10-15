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

        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in _context.Pages orderby p.Sorting select p;

            var pagesList = await pages.ToListAsync();

            return View(pagesList);
        }
    }
}
