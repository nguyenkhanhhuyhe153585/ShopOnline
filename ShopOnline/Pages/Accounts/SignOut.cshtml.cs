using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Common;

namespace ShopOnline.Pages.Accounts
{
    public class SignOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove(Const.ACCOUNT_SESSION);
            HttpContext.Response.Cookies.Delete(Const.COOKIE_TOKEN);
            return RedirectToPage("/index");
        }
    }
}
