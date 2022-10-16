using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public void OnGet(int orderId)
        {
            orderInfo = db.Orders.Where(o => o.OrderId == orderId)
                .Include(e => e.OrderDetails).ThenInclude(e => e.Product).SingleOrDefault();
        }

    }
}
