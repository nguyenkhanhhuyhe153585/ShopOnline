using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorPage.Common;
using MyRazorPage.Models;

namespace MyRazorPage.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private PRN221DBContext dBContext;
        public IndexModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public List<Category> categories { get; set; }
        public List<Models.Product> products { get; set; }
        private int pageSize = 12;
        public decimal totalPage;

        public void OnGet(int categoryId, string sortBy, int pageNum)
        {
            pageNum = Utils.PageLimit(pageNum);
            categories = dBContext.Categories.ToList();
            
            IOrderedQueryable<Models.Product> orderCategory;
            IQueryable<Models.Product> queryProduct = dBContext.Products.Where(e => e.CategoryId.Equals(categoryId));

            if (sortBy == null || sortBy.Equals("priceAsc"))
            {
                orderCategory = queryProduct.OrderBy(e => e.UnitPrice);
            }
            else
            {
                orderCategory = queryProduct.OrderByDescending(e => e.UnitPrice);
            }

            (queryProduct, totalPage) = Utils.Page<Models.Product>(orderCategory, pageSize, pageNum);

            products = queryProduct.ToList();
            ViewData["sortBy"] = sortBy;
        }
    }
}
