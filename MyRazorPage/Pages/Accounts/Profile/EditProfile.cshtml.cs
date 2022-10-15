using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorPage.Models;
using System.Text.Json;

namespace MyRazorPage.Pages.Accounts.Profile
{
    public class EditProfileModel : PageModel
    {
        private readonly PRN221DBContext dBContext;

        [BindProperty]
        public Models.Account Account { get; set; }
        [BindProperty]
        public Models.Customer Customer { get; set; }

        public EditProfileModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        private Models.Account GetAccountFromSession()
        {
            string accountString = HttpContext.Session.GetString("CustSession");
            if (accountString != null)
            {
                return JsonSerializer.Deserialize<Models.Account>(accountString);
            }
            return null;
        }

        public void OnGet()
        {

            Account = GetAccountFromSession();

            Customer = dBContext.Customers.Find(Account.CustomerId);
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Account.Password");
            ModelState.Remove("Customer.CustomerId");
            if (ModelState.IsValid)
            {
                Models.Account accountFromSession = GetAccountFromSession();
                Models.Account accFindByEmail = dBContext.Accounts.SingleOrDefault(e => (e.Email.Equals(Account.Email) && !e.AccountId.Equals(accountFromSession.AccountId)));
                if (accFindByEmail != null)
                {
                    ViewData["msg"] = "Email was used";
                    return Page();
                }

                Models.Account accountFromDB = dBContext.Accounts.Find(accountFromSession.AccountId);
                accountFromDB.Email = Account.Email.Trim();
                //if (Account.Password.Trim().Length > 0)
                //{
                //    accountFromDB.Password = Account.Password.Trim();
                //};
                dBContext.Accounts.Update(accountFromDB);
                Models.Customer customerFromDB = dBContext.Customers.Find(accountFromSession.CustomerId);
                customerFromDB.CompanyName = Customer.CompanyName.Trim();
                customerFromDB.ContactName = Customer.ContactName.Trim();
                customerFromDB.ContactTitle = Customer.ContactTitle.Trim();
                customerFromDB.Address = Customer.Address.Trim();
                dBContext.Customers.Update(customerFromDB);

                await dBContext.SaveChangesAsync();
                ViewData["msg"] = "Update successful";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
