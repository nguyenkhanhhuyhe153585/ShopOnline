using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Pages
{
    public class ErrorPageModel : PageModel
    {
        public void OnGet(string code)
        {
            if(code == null)
            {
                code = "404";
            }
            ViewData["code"] = code;
        }
    }
}
