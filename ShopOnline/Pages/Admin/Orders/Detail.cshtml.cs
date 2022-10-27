using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin.Orders
{
    public class DetailModel : PageModel
    {
        private readonly PRN221DBContext db;

        public DetailModel(PRN221DBContext db)
        {
            this.db = db;
        }

        public Order orderInfo;

        public IActionResult OnGet(int orderId)
        {
            if (SessionUtils.isAdminSession(HttpContext.Session))
            {
                orderInfo = db.Orders.Where(o => o.OrderId == orderId)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.Product).SingleOrDefault();
                return Page();
            }else
            {
                return Redirect("/errorpage?code=401");
            }
        }

    }
}
