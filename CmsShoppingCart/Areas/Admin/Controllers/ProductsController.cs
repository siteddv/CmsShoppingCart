using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartContext _context;

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
    }
}
