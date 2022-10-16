using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet(DateTime? txtStartOrderDate, DateTime? txtEndOrderDate, int pageNum)
        {
            IQueryable<Order> query = dBContext.Orders.Where(e => (txtStartOrderDate == null ? true : e.OrderDate >= txtStartOrderDate)
                && (txtEndOrderDate == null ? true : e.OrderDate <= txtEndOrderDate));
            (query, totalPage) = Utils.Page(query, pageSize, pageNum);

            orders = query.ToList();
        }
    }
}
