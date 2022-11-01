using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin.Employees
{
    public class IndexModel : PageModel
    {
        private readonly ShopOnline.Models.PRN221DBContext _context;

        public IndexModel(ShopOnline.Models.PRN221DBContext context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get; set; } = default!;

        private int pageSize = 10;
        public decimal totalPage = 0;

        public async Task OnGetAsync(int pageNum)
        {
            if (_context.Employees != null)
            {
                IQueryable<Employee> query = _context.Employees;
                (query, totalPage) = Utils.Page(query, pageSize, pageNum);
                Employee = await query.Include(e => e.Department).ToListAsync();
            }
        }
    }
}
