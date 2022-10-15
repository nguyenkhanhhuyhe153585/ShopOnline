using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using MyRazorPage.Models;
using MyRazorPage.SignalRLab;

namespace MyRazorPage.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly MyRazorPage.Models.PRN221DBContext _context;
        private readonly IHubContext<SignalrServer> signalR;

        public CreateModel(MyRazorPage.Models.PRN221DBContext context, IHubContext<SignalrServer> signalR)
        {
            _context = context;
            this.signalR = signalR;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return Page();
        }

        [BindProperty]
        public Models.Product Product { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
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
