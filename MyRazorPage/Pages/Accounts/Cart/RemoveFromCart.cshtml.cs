using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorPage.Common;
using MyRazorPage.Models;
using System.Text.Json;

namespace MyRazorPage.Pages.Accounts.Cart
{
    public class RemoveFromCartModel : PageModel
    {

        public List<OrderDetail> orderDetailsCart { get; set; }

        public IActionResult OnGet(int productId)
        {
            orderDetailsCart = SessionUtils.GetCartInfo(HttpContext.Session);
            OrderDetail orderToRemove = orderDetailsCart.SingleOrDefault(e => e.ProductId == productId);
            if(orderToRemove != null)
            {
                orderDetailsCart.Remove(orderToRemove);
            }
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(orderDetailsCart));
            return RedirectToPage("Index");
        }
    }
}
