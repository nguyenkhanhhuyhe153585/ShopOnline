using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Pages.Accounts
{
    public class SignOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("CustSession");
            return RedirectToPage("/index");
        }
    }
}
