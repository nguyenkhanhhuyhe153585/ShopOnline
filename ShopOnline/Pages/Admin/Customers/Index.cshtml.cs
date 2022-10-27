using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ShopOnline.Models.PRN221DBContext _context;

        public IndexModel(ShopOnline.Models.PRN221DBContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            if (_context.Customers != null)
            {
                Customer = await _context.Customers.ToListAsync();
                
            }
            return Page();
        }
    }
}
