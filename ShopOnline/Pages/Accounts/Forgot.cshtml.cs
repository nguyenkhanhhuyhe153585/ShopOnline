using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

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
            Account account = await db.Accounts.Where(e => e.Email.Equals(email)).SingleOrDefaultAsync();
            ViewData["email"] = email;
            if(account == null)
            {
                ViewData["msg"] = "Email not exist. Try again.";
                
                return Page();
            }
             return Page(); 
        }
    }
}
