using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorPage.Models;

namespace MyRazorPage.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly MyRazorPage.Models.PRN221DBContext _context;

        public IndexModel(MyRazorPage.Models.PRN221DBContext context)
        {
            _context = context;
        }

        public IList<Models.Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products
                .Include(p => p.Category).ToListAsync();
            }
        }
    }
}
