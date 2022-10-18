using ShopOnline.Models;
using System.Text.Json;

namespace ShopOnline.Common
{
    public class SessionUtils
    {
        public static Dictionary<int, OrderDetail> GetCartInfo(ISession session)
        {
            string cart = session.GetString("Cart");
            if (cart == null)
            {
                return new Dictionary<int, OrderDetail>();
            }
            else
            {
                return JsonSerializer.Deserialize<Dictionary<int, OrderDetail>>(cart);
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
            if(account == null)
            {
                return false;
            }
            return account.Role == 1;
        }
    }
}
