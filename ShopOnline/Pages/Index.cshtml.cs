using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Models;
using System.Linq;
using System.Text.Json;

namespace ShopOnline.Pages
{
    public class IndexModel : PageModel
    {
        private PRN221DBContext dBContext;

        public IndexModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public List<Product> Hot { get; set; }
        [BindProperty]
        public List<Product> BestSale { get; set; }
        [BindProperty]
        public List<Product> New { get; set; }
        [BindProperty]
        public List<Category> Categories { get; set; }

        public IActionResult OnGet(int? categoryId)
        {
            Categories = dBContext.Categories.ToList();
            if (categoryId == null)
            {
                Hot = (from orderdetail in dBContext.OrderDetails
                       join product in dBContext.Products on orderdetail.ProductId
                       equals product.ProductId
                       join order in dBContext.Orders on orderdetail.OrderId equals order.OrderId
                       where product.UnitsInStock > 0 && product.Discontinued == false
                       group order by product.ProductId into g
                       orderby g.Count() descending
                       select new { ProductId = g.Key }
                        ).Take(4).Join(dBContext.Products, e => e.ProductId, i => i.ProductId, (e, i) => i).ToList();
                BestSale = (from orderdetail in dBContext.OrderDetails
                            join product in dBContext.Products on orderdetail.ProductId
                            equals product.ProductId
                            where product.UnitsInStock > 0 && product.Discontinued == false
                            group orderdetail by product.ProductId into g
                            orderby g.Sum(g => g.Quantity) descending
                            select new { ProductId = g.Key }
                        ).Take(4).Join(dBContext.Products, e => e.ProductId, i => i.ProductId, (e, i) => i).ToList();
                New = (from product in dBContext.Products
                       where product.UnitsInStock > 0 && product.Discontinued == false
                       orderby product.ProductId descending
                       select product).Take(4).ToList();
            }
            else
            {
                Hot = (from orderdetail in dBContext.OrderDetails
                       join product in dBContext.Products on orderdetail.ProductId
                       equals product.ProductId
                       join order in dBContext.Orders on orderdetail.OrderId equals order.OrderId
                       where product.CategoryId.Equals(categoryId) && product.UnitsInStock > 0 && product.Discontinued == false
                       group order by product.ProductId into g
                       orderby g.Count() descending
                       select new { ProductId = g.Key }
                        ).Take(4).Join(dBContext.Products, e => e.ProductId, i => i.ProductId, (e, i) => i).ToList();
                BestSale = (from orderdetail in dBContext.OrderDetails
                            join product in dBContext.Products on orderdetail.ProductId
                            equals product.ProductId
                            where product.CategoryId.Equals(categoryId) && product.UnitsInStock > 0 && product.Discontinued == false
                            group orderdetail by product.ProductId into g
                            orderby g.Sum(g => g.Quantity) descending
                            select new { ProductId = g.Key }
                        ).Take(4).Join(dBContext.Products, e => e.ProductId, i => i.ProductId, (e, i) => i).ToList();
                New = (from product in dBContext.Products
                       where product.CategoryId.Equals(categoryId) && product.UnitsInStock > 0 && product.Discontinued == false
                       orderby product.ProductId descending
                       select product).Take(4).ToList();
            }
            return Page();
        }
    }
}
