using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EadProjectShoppingApp.Models.Extended
{
    public class CartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}