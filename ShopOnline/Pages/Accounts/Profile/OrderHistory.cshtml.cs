using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;
using System.Linq;

namespace ShopOnline.Pages.Accounts.Profile
{
    public class OrderHistoryModel : PageModel
    {
        private PRN221DBContext dBContext;

        public OrderHistoryModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }


        public List<Order> Orders { get; set; }

        public string CustomerContactName { get; set; }

        public IActionResult OnGet()

        {
            Account account = Utils.GetAccountFromSession(HttpContext.Session);
            if (account != null)
            {
                Orders = dBContext.Orders.Include(e => e.OrderDetails).ThenInclude(e => e.Product).Where(e => e.CustomerId == account.CustomerId).ToList();


                var customerFromDB = dBContext.Customers.Find(account.CustomerId);
                if (customerFromDB != null)
                {
                    CustomerContactName = customerFromDB.ContactName;
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Accounts/SignIn");
            }

        }
    }
}
