using ShopOnline.Models;
using System.Text.Json;

namespace ShopOnline.Common
{
    public class SessionUtils
    {
        public static List<OrderDetail> GetCartInfo(ISession session)
        {
            string cart = session.GetString("Cart");
            if (cart == null)
            {
                return new List<OrderDetail>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<OrderDetail>>(cart);
            }
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

        public static bool isAdmin(Account account)
        {
            return account.Role == 2;
        }
    }
}
