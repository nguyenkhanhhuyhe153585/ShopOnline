using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext dBContext;
        public IndexModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        private int pageSize = 10;
        public decimal totalPage = 0;
        public List<Order> orders = new List<Order>();

        public IActionResult OnGet(DateTime? txtStartOrderDate, DateTime? txtEndOrderDate, int pageNum)
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            if (txtStartOrderDate != null)
            {
                txtStartOrderDate = Utils.StartOfDay((DateTime)txtStartOrderDate);
            }
            if (txtEndOrderDate != null)
            {
                txtEndOrderDate = Utils.EndOfDay((DateTime)txtEndOrderDate);
            }

            IQueryable<Order> query = dBContext.Orders.Where(e =>
                    (txtStartOrderDate == null ? true : e.OrderDate >= txtStartOrderDate)
                && (txtEndOrderDate == null ? true : e.OrderDate <= txtEndOrderDate)).OrderByDescending(e => e.OrderDate);
            (query, totalPage) = Utils.Page(query, pageSize, pageNum);

            orders = query.Include(e => e.Employee).Include(e => e.Customer).ToList();

            ViewData["txtStartOrderDate"] = txtStartOrderDate?.ToString("yyyy-MM-dd");
            ViewData["txtEndOrderDate"] = txtEndOrderDate?.ToString("yyyy-MM-dd");
            return Page();
        }
    }
}
