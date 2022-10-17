using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Common;
using ShopOnline.Models;
using System.Text.Json;

namespace ShopOnline.Pages.Accounts
{
    public class SignInModel : PageModel
    {

        private readonly PRN221DBContext dBContext;
        [BindProperty]
        public Account Account { get; set; }

        public SignInModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("CustSession") != null)
            {
                HttpContext.Session.Remove("CustSession");
                return RedirectToPage("/index");
            }
            HttpContext.Session.Remove("CustSession");
            return Page();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Account account = dBContext.Accounts.SingleOrDefault(e => e.Email.Equals(Account.Email));
                if (account == null)
                {
                    ViewData["msg"] = "Account invalid. Try again";
                    return Page();
                }
                else
                {
                    if (account.Password.Equals(Account.Password))
                    {
                        HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(account));

                        if (SessionUtils.isAdmin(account))
                        {
                            return Redirect("/admin/dashboard");
                        }

                        return Redirect("/index");
                    }
                    ViewData["msg"] = "Invalid Password";
                    return Page();
                }
            }
            return Page();
        }
    }
}
