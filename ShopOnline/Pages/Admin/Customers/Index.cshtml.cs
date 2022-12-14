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

        private int pageSize = 10;
        public decimal totalPage = 0;

        public async Task<IActionResult> OnGetAsync(int pageNum, string search)
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            if (_context.Customers != null)
            {
                ViewData["search"] = search;   
                IQueryable<Customer> query = _context.Customers.Where(e=> (search == null)? true : e.ContactName.Contains(search));
                (query, totalPage) = Utils.Page(query, pageSize, pageNum);
                Customer = await query.ToListAsync();
            }
            return Page();
        }
    }
}
