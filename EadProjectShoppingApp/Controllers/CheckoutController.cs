using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EadProjectShoppingApp.Models;

namespace EadProjectShoppingApp.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        private Shopping_Store_DataBaseEntities db = new Shopping_Store_DataBaseEntities();
        const String PromoCode = "FREE";
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new CustomerOrder();

            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Email = User.Identity.Name;
                    order.DateCreated = DateTime.Now;

                    db.CustomerOrders.Add(order);
                    db.SaveChanges();

                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    db.SaveChanges();//we have received the total amount lets update it

                    return RedirectToAction("Complete", new { id = order.Id });
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return View(order);
            }
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.CustomerOrders.Any(
                o => o.Id == id &&
                     o.Email == User.Identity.Name
            );

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}