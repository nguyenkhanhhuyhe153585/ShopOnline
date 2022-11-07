using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;
using System.Text;

namespace ShopOnline.Pages.Accounts
{
    public class ForgotModel : PageModel
    {
        private readonly PRN221DBContext db;

        public ForgotModel(PRN221DBContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(string email)
        {
             return Page(); 
        }

        public async Task<IActionResult> OnPostAsync(string email)
        {
            Account account = await db.Accounts.Where(e => e.Email.Equals(email)).Include(e => e.Customer).SingleOrDefaultAsync();
            ViewData["email"] = email;
            if (account == null)
            {
                ViewData["msg"] = "Email not exist. Try again.";
                return Page();
            }
            string token = SessionUtils.EncodeJWTToken(account);
            var builder = WebApplication.CreateBuilder();
            string domain = builder.Configuration["Server:Domain"];

            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(domain);
            urlBuilder.Append("accounts/resetpassword?token=");
            urlBuilder.Append(token);
            await Utils.Email(account.Email, "Reset Password",
                new HTMLTemplate().MailResetPassword(account, urlBuilder.ToString())
                , null);
            ViewData["msg"] = "Mail sended!";
            return Page();
        }
    }
}
