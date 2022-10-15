using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorPage.Models;

namespace MyRazorPage.Pages.Products
{
    public class DetailModel : PageModel
    {
        private PRN221DBContext dBContext;
        public DetailModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public Product Product;

        public IActionResult OnGet(int productId)
        {
            Product = dBContext.Products.Include(e => e.Category).SingleOrDefault(e => e.ProductId == productId);
            return Page();
        }
    }
}
