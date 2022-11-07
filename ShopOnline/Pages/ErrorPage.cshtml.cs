using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Pages
{
    public class ErrorPageModel : PageModel
    {
        public void OnGet(string code, string message)
        {
            if(code == null)
            {
                code = "404";
            }
            ViewData["code"] = code;
            ViewData["msg"] = message;
        }
    }
}
