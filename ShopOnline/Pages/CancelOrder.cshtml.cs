using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ShopOnline.Common;
using ShopOnline.Models;
using ShopOnline.SignalRLab;

namespace ShopOnline.Pages
{
    public class CancelOrderModel : PageModel
    {
        private readonly PRN221DBContext db;
        private readonly IHubContext<SignalrServer> signalR;

        public CancelOrderModel(PRN221DBContext db, IHubContext<SignalrServer> signalR)
        {
            this.db = db;
            this.signalR = signalR;
        }

        public async Task<IActionResult> OnGet(int? orderId)
        {
            if (orderId == null)
            {
                return Redirect("/errorpage?code=403");
            }
            Account currentUser = SessionUtils.GetAccountFromSession(HttpContext.Session);
            if(currentUser == null)
            {
                return Redirect("/accounts/signin");
            }
            
            Order canceleOrder = db.Orders.Where(o => o.OrderId == orderId).SingleOrDefault();
            if(canceleOrder.CustomerId.Equals(currentUser.CustomerId) || SessionUtils.isAdmin(currentUser))
            {
                canceleOrder.RequiredDate = null;
                db.Orders.Update(canceleOrder);
                await db.SaveChangesAsync();
                await signalR.Clients.All.SendAsync("RefreshOrderList");
                return Redirect(Request.Headers.Referer);             
            }
            return Redirect("/errorpage?code=401");
        }
    }
}
