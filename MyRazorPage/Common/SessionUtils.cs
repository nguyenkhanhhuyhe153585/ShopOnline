using MyRazorPage.Models;
using System.Text.Json;

namespace MyRazorPage.Common
{
    public class SessionUtils
    {
        public static List<OrderDetail> GetCartInfo(ISession session)
        {
            string cart = session.GetString("Cart");
            if(cart == null)
            {
                return new List<OrderDetail>();
            }else
            {
                return JsonSerializer.Deserialize<List<OrderDetail>>(cart);
            }
        }

        public static Models.Account GetAccountFromSession(ISession session)
        {
            string accountString = session.GetString("CustSession");
            if (accountString != null)
            {
                return JsonSerializer.Deserialize<Models.Account>(accountString);
            }
            return null;
        }
    }
}
