using Microsoft.AspNetCore.SignalR;

namespace MyRazorPage.SignalRLab
{
    public class SignalrServer : Hub
    {
        public void HasNewData()
        {
            Clients.All.SendAsync("ReloadProduct");
        }
    }
}
