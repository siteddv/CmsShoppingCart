using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Model
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
