using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Accounts
{
    public class ResetpasswordModel : PageModel
    {
        private readonly PRN221DBContext db;

        public ResetpasswordModel(PRN221DBContext db)
        {
            this.db = db;
        }

        public ActionResult OnGet(string token)
        {

            try
            {
                SessionUtils.DecodeJWTTokenGetName(token);
            }
            catch (Exception ex)
            {
                ViewData["msg"] = "Invalid URL";
                return Redirect("/errorpage?code=403");
            }

            ViewData["token"] = token;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string token, string password, string confirmpassword)
        {
            ViewData["token"] = token;
            try
            {
                var claims = SessionUtils.DecodeJWTTokenGetName(token);
                var account = db.Accounts.Find(int.Parse(claims["Sub"]));
                if (account == null)
                {
                    throw new Exception("Bad token");
                }
                password = password.Trim();
                if (password.Length == 0)
                {
                    ViewData["msg"] = "Password empty";
                    return Page();
                }
                account.Password = SessionUtils.PasswordUtils.PasswordHasher(password.Trim());
                db.Accounts.Update(account);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return Redirect("errorpage?code=403&message=BadRequest");
            }
            ViewData["msg"] = "Password reset successful";
            return Page();
        }
    }
}
