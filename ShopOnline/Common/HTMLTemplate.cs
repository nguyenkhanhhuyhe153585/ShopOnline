using ShopOnline.Models;
using System.Text;

namespace ShopOnline.Common
{
    public class HTMLTemplate
    {

        public string MailConfrimOrder(Order order)
        {
            StringBuilder mailBuilder = new StringBuilder();
            mailBuilder.Append($"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n" +
                $"<meta charset=\"UTF-8\">\r\n" +
                $"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n" +
                $"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
                $"<title>Document</title>\r\n</head>\r\n<body>\r\n" +
                $"<h1>Order confirmation</h1>\r\n" +
                $"<h3>Your order #{order.OrderId}</h3>\r\n\r\n" +
                $"<p>Hey {order.Customer.ContactName}</p>\r\n" +
                $"<p>You have an order from ShopOnline</p>\r\n</body>\r\n</html>");
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

        // For generate invoice
        public static string GetHTMLOrderString(Order order)
        {
            var sb = new StringBuilder();
            sb.Append(@$"<!DOCTYPE html><html><body><h1>Order #{order.OrderId}</h1><div>
        <span>{order.Customer.ContactName}</span>
        <br/>
        <span{order.Customer.CompanyName}</span>
        <br/>
        <span>{order.Customer.Address}</span>
    </div>
    <div>

    </div>
    <div>
        <table border>
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Quantity</th>
                    <th>UnitPrice</th>
                    <th>Discount</th>
                </tr>
            </thead>
            <tbody>");
            foreach (var orderDetail in order.OrderDetails)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", orderDetail.Product.ProductName, orderDetail.Quantity, orderDetail.UnitPrice, orderDetail.Discount);
            }
            sb.AppendFormat(@"</tbody>
        </table>
        <span>Total: {0}</span>
    </div>
</body>

</html>", order.Freight);
            return sb.ToString();
        }
    }
}
