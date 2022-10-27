using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Common;
using ShopOnline.Models;
using ShopOnline.SignalRLab;

namespace ShopOnline.Pages.Admin.Products
{
    public class UpdateModel : PageModel
    {
        private readonly PRN221DBContext _context;
        private readonly IHubContext<SignalrServer> signalR;

        public UpdateModel(PRN221DBContext context, IHubContext<SignalrServer> signalR)
        {
            _context = context;
            this.signalR = signalR;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        private string pageBeforeUrl = string.Empty;

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

            pageBeforeUrl = Request.GetDisplayUrl();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("URL" + pageBeforeUrl, Console.BackgroundColor);

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await signalR.Clients.All.SendAsync("LoadProductOnChange");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Redirect("/admin/products");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
