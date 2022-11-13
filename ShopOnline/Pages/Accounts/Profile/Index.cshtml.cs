using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Accounts.Profile
{
    public class IndexModel : PageModel
    {

        private readonly PRN221DBContext dBContext;

        public IndexModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        public IActionResult OnGet()
        {
            Account = Utils.GetAccountFromSession(HttpContext.Session);
            if (Account == null)
            {
                return RedirectToPage("/Index");
            }
            Customer = dBContext.Customers.Find(Account.CustomerId);
            return Page();
        }
    }
}
