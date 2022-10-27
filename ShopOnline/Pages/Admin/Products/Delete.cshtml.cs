using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;
using ShopOnline.SignalRLab;

namespace ShopOnline.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {
        private readonly PRN221DBContext _context;
        private readonly IHubContext<SignalrServer> signalR;

        public DeleteModel(PRN221DBContext context, IHubContext<SignalrServer> signalR)
        {
            _context = context;
            this.signalR = signalR;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(e=>e.Category).SingleOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            var orderDetails = _context.OrderDetails.Where(e => e.ProductId == id);

            if(orderDetails.Count() > 0)
            {
                ViewData["msg"] = "Product has orders";
                return Page();
            }

            if (product != null)
            {
                Product = product;
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();
                await signalR.Clients.All.SendAsync("LoadProductOnChange");
            }
            
            return RedirectToPage("./index");
        }
    }
}