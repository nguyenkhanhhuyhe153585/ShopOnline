using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext db;

        public IndexModel(PRN221DBContext context)
        {
            db = context;
        }

        public IList<Product> Product { get; set; } = default!;

        public decimal totalPage = 0;
        public List<Product> products;
        public List<Category> categories;
        private int pageSize = 10;
        public async Task OnGetAsync(int categoryId, string search, int pageNum)
        {
            ViewData["categoryId"] = categoryId;
            ViewData["search"] = search;
            if (search == null)
            {
                search = "";
            }
            categories = db.Categories.ToList();
            if (db.Products != null)
            {
                IQueryable<Product> query = db.Products.Where(e => ((categoryId == 0) ? true : e.CategoryId == categoryId)
                    && ((search.Length == 0) ? true : e.ProductName.Contains(search))).OrderByDescending(e => e.ProductId);
                (query, totalPage) = Utils.Page(query, pageSize, pageNum);
                products = query.Include(e => e.Category).ToList();
            }
        }
    }
}
