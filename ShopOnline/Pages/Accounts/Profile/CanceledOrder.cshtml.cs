using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Accounts.Profile
{
    public class CanceledOrderModel : PageModel
    {
        private readonly PRN221DBContext dBContext;

        public CanceledOrderModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public List<Order> Orders;
        public string CustomerContactName;
        public IActionResult OnGet()
        {
            Account account = Utils.GetAccountFromSession(HttpContext.Session);
            if (account != null)
            {
                Orders = dBContext.Orders.Include(e => e.OrderDetails).ThenInclude(e => e.Product)
                    .Where(e => e.CustomerId == account.CustomerId && (e.RequiredDate == null && e.ShippedDate == null))
                    .OrderByDescending(e => e.OrderDate).ToList();


                var customerFromDB = dBContext.Customers.Find(account.CustomerId);
                if (customerFromDB != null)
                {
                    CustomerContactName = customerFromDB.ContactName;
                }
                return Page();
            }
            else
            {
                return Redirect("/accounts/signin");
            }
        }
    }
}
