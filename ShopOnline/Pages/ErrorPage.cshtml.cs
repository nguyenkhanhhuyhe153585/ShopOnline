using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Pages
{
    public class ErrorPageModel : PageModel
    {
        public void OnGet(int code)
        {
            ViewData["code"] = code;
        }
    }
}
