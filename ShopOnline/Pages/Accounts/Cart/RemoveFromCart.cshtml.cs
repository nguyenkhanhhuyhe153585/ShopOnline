using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Common;
using ShopOnline.Models;
using System.Text.Json;

namespace ShopOnline.Pages.Accounts.Cart
{
    public class RemoveFromCartModel : PageModel
    {

        public Dictionary<int, OrderDetail> orderDetailsCart { get; set; }

        public IActionResult OnGet(int productId)
        {
            orderDetailsCart = SessionUtils.GetCartInfo(HttpContext.Session);
            
            if (orderDetailsCart.ContainsKey(productId))          
            {    
                orderDetailsCart.Remove(productId);
            }
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(orderDetailsCart));
            return RedirectToPage("Index");
        }
    }
}
