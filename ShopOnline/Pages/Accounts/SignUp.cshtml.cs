using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using ShopOnline.Models;
using ShopOnline.Common;
using ShopOnline.Models;
using System;
using System.Text.Json;


namespace ShopOnline.Pages.Accounts
{
    public class SignUpModel : PageModel
    {
        private readonly PRN221DBContext dBContext;
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Account Account { get; set; }

        public SignUpModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public void OnGet()
        {
        }

        [HttpPost]
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var acc = await dBContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Account.Email));
                if (acc != null)
                {
                    ViewData["msg"] = "This email exist";
                    return Page();
                }
                string customerId = "";
                customerId = Utils.RandomCustId(dBContext.Customers);


                var customer = new Customer
                {
                    CustomerId = customerId,
                    CompanyName = Customer.CompanyName,
                    ContactName = Customer.ContactName,
                    ContactTitle = Customer.ContactTitle,
                    CreateDate = DateTime.Now,
                    Address = Customer.Address
                };
                await dBContext.Customers.AddAsync(customer);
                var account = new Account
                {
                    CustomerId = customer.CustomerId,
                    Email = Account.Email,
                    Password = SessionUtils.PasswordHasher(Account.Password),
                    Role = 2
                };
                await dBContext.Accounts.AddAsync(account);
                await dBContext.SaveChangesAsync(); 

                //HttpContext.Session.SetString("account", JsonSerializer.Serialize(account));

                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }
    }
}
