using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using System.Text.Json;

namespace ShopOnline.Common
{
    public class Utils
    {
        public static string RandomChar(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomCustId(DbSet<Customer> customers)
        {
            string customerId;
            do
            {
                customerId = RandomChar(5).ToUpper();
            } while (customers.SingleOrDefault(e => e.CustomerId.Equals(customerId)) != null);
            return customerId;
        }

        public static Account GetAccountFromSession(ISession session)
        {
            string accountString = session.GetString("CustSession");
            if (accountString != null)
            {
                return JsonSerializer.Deserialize<Account>(accountString);
            }
            return null;
        }

        public static (IQueryable<T> query, decimal totalPage) Page<T>(IQueryable<T> en, int pageSize, int page)
        {
            page = PageLimit(page);
            return (en.Skip(page * pageSize).Take(pageSize), Math.Ceiling((decimal)en.Count() / pageSize));
        }

        public static int PageLimit(int pageNum)
        {
            pageNum -= 1;
            if (pageNum <= 0)
            {
                pageNum = 0;
            }
            return pageNum;
        }
    }


}
