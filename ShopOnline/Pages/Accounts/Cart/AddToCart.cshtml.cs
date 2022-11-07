using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;
using System.Text.Json;

namespace ShopOnline.Pages.Accounts.Cart
{
    public class AddToCartModel : PageModel
    {

        private readonly PRN221DBContext dBContext;

        public AddToCartModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public Customer Customer { get; set; }
        public Dictionary<int, OrderDetail> orderDetailsCard;

        public IActionResult OnGet(int productId, bool isBuyNow)
        {
            Account account = SessionUtils.GetAccountFromSession(HttpContext.Session);
            if (account == null)
            {
                Customer = new Customer();
            }
            else
            {
                Customer = dBContext.Customers.Find(account.CustomerId);
            }
            if (productId == 0)
            {
                return RedirectToPage("Index");
            }
            orderDetailsCard = SessionUtils.GetCartInfo(HttpContext);
            Product productFromDB = dBContext.Products.Find(productId);
            if (productFromDB == null)
            {
                return null;
            }
            if (!orderDetailsCard.ContainsKey(productId))
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Product = productFromDB,
                    ProductId = productId,
                    Quantity = 1,
                    UnitPrice = (decimal)productFromDB.UnitPrice
                };
                orderDetailsCard.Add(productId, orderDetail);
            }          
            else
            {
                OrderDetail orderDetailFromCart = orderDetailsCard[productId];
                orderDetailFromCart.Quantity++;
            }
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize(orderDetailsCard), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            if (isBuyNow)
                return RedirectToPage("Index");
            else
            {
                return Redirect(Request.Headers.Referer);
            }
        }
    }
}
