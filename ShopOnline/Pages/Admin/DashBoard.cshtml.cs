using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin
{
    public class DashBoardModel : PageModel
    {
        private readonly PRN221DBContext dBContext;
        public DashBoardModel(PRN221DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public float weeklySale = 0;
        public float totalOrders = 0;
        public int totalCustomerHasAccount = 0;
        public int totalGuest = 0;
        public int totalCustomer = 0;
        public int newCustomer30days = 0;

        public string orders12monthsJson;

        public void OnGet()
        {
            DateTime currentDateTime = DateTime.Now;

            DateTime monday, sunday;
            monday = currentDateTime.AddDays(-(int)currentDateTime.DayOfWeek + 1);
            sunday = currentDateTime.AddDays(7 - (int)currentDateTime.DayOfWeek);



            weeklySale = (from o in dBContext.Orders
                          where o.OrderDate >= monday && o.OrderDate <= sunday
                          select (float)o.Freight).Sum();

            totalOrders = (from o in dBContext.Orders
                           select (float)o.Freight).Sum();

            totalCustomerHasAccount = (from customer in dBContext.Customers
                                       join account in dBContext.Accounts on customer.CustomerId equals account.CustomerId
                                       select customer).Count();

            totalGuest = (from customer in dBContext.Customers
                          join account in dBContext.Accounts on customer.CustomerId equals account.CustomerId into ac
                          from subaccount in ac.DefaultIfEmpty()
                          where subaccount == null
                          select customer).Count();

            totalCustomer = (from customer in dBContext.Customers
                             select customer).Count();

            newCustomer30days = (from customer in dBContext.Customers
                                 where customer.CreateDate.Value.Month == currentDateTime.Month
                                    && customer.CreateDate.Value.Year == currentDateTime.Year
                                 select customer).Count();

            orders12monthsJson = (from order in dBContext.Orders
                                  where order.OrderDate.Value.Year == 1997
                                  group order by order.OrderDate.Value.Month into orderMonth
                                  orderby orderMonth.Key ascending
                                  select orderMonth.Select(o => o.OrderId).Count()

                     ).ToList().ToJson();


        }


    }
}
