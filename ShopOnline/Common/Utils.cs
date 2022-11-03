using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Text;

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

        public static DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        // For sendmail
        public static async Task Email(string mailTo, string body, Attachment attachment)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("area1110@outlook.com");
                message.To.Add(new MailAddress(mailTo));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
                if (attachment != null)
                {
                    message.Attachments.Add(attachment);
                }
                smtp.Port = 587;
                smtp.Host = "smtp-mail.outlook.com"; //for outlook mail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("area1110@outlook.com", "Khanh@outlook1huy");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        // For generate invoice
        public static string GetHTMLOrderString(Order order)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>LastName</th>
                                        <th>Age</th>
                                        <th>Gender</th>
                                    </tr>");
            foreach (var orderDetail in order.OrderDetails)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", orderDetail.Product.ProductName, orderDetail.Quantity, orderDetail.UnitPrice, orderDetail.Discount);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }


}
