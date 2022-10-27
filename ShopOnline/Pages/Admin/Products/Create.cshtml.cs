using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using ShopOnline.Common;
using ShopOnline.Models;
using ShopOnline.SignalRLab;

namespace ShopOnline.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly PRN221DBContext _context;
        private readonly IHubContext<SignalrServer> signalR;

        public CreateModel(PRN221DBContext context, IHubContext<SignalrServer> signalR)
        {
            _context = context;
            this.signalR = signalR;
        }

        public IActionResult OnGet()
        {
            if (!SessionUtils.isAdminSession(HttpContext.Session))
            {
                return Redirect("/errorpage?code=401");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

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

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            await signalR.Clients.All.SendAsync("LoadProductOnChange");

            return RedirectToPage("./Index");
        }
    }
}
