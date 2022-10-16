using Microsoft.AspNetCore.SignalR;

namespace ShopOnline.SignalRLab
{
    public class SignalrServer : Hub
    {
        public void HasNewData()
        {
            Clients.All.SendAsync("ReloadProduct");
        }
    }
}
