using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopOnline.Common;
using ShopOnline.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                Account account = dBContext.Accounts.Where(e => e.Customer.IsActive == true).SingleOrDefault(e => e.Email.Equals(Account.Email));
                if (account == null)
                {
                    ViewData["msg"] = "Account invalid. Try again";
                    return Page();
                }
                else
                {
                    if (SessionUtils.PasswordCompare(Account.Password, account.Password))
                    {

                        // Add jwt to cookies
                        HttpContext.Response.Cookies.Append("Token", SessionUtils.EncodeJWTToken(account));

                        if (SessionUtils.isAdmin(account))
                        {
                            HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(account));
                            return Redirect("/admin/dashboard");
                        }
                        else if (account.Customer?.IsActive == false)
                        {
                            ViewData["msg"] = "Invalid Password";
                            return Page();
                        }
                        else
                        {
                            HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(account));
                            return Redirect("/index");
                        }
                    }
                    ViewData["msg"] = "Invalid Password";
                    return Page();
                }
            }
            return Page();
        }

    }
}
