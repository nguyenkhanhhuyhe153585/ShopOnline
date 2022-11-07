using ShopOnline.Models;
using System.Text;

namespace ShopOnline.Common
{
    public class HTMLTemplate
    {

        public string MailConfrimOrder(Order order)
        {
            StringBuilder  mailBuilder = new StringBuilder();
            mailBuilder.Append($"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n" +
                $"<meta charset=\"UTF-8\">\r\n" +
                $"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n" +
                $"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
                $"<title>Document</title>\r\n</head>\r\n<body>\r\n" +
                $"<h1>Order confirmation</h1>\r\n" +
                $"<h3>Your order #{order.OrderId}</h3>\r\n\r\n" +
                $"<p>Hey {order.Customer.ContactName}</p>\r\n" +
                $"<p></p>\r\n</body>\r\n</html>");
            return mailBuilder.ToString();
        }

        public string MailResetPassword(Account account, string urlToken)
        {
            StringBuilder mailBuilder = new StringBuilder();
            mailBuilder.Append($" <h3>Hi, {account.Customer.ContactName}</h3>\r\n" +
                $"<p>This is email reset password from ShopingOnline\r\n" +
                $"</p>\r\n    <p>Please goto the link below for reset your password</p>\r\n" +
                $"<p><a href=\"{urlToken}\">{urlToken}</a></p>");

            return mailBuilder.ToString();
        }
    }
}
