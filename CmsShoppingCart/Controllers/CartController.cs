using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public CartController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        //GET /cart
        public IActionResult Index()
        {
            var carts = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartVM = new CartViewModel
            {
                CartItems = carts,
                GrandTotal = carts.Sum(c => c.Price * c.Quantity)
            };

            return View(cartVM);
        }
    }
}
