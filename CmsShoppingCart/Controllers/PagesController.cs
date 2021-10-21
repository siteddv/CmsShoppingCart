using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class PagesController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public PagesController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        // GET / or /slug
        public async Task<IActionResult> Page(string slug)
        {
            var page = await _context.Pages.Where(p => p.Slug.Equals(slug)).FirstOrDefaultAsync();
            var homePage = await _context.Pages.Where(p => p.Slug.Equals("home")).FirstOrDefaultAsync();


            if(slug == null)
                return View(homePage);

            if (page == null)
                return NotFound();

            return View();
        }
    }
}
