using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ShopOnline.Common;
using ShopOnline.Models;
using ShopOnline.SignalRLab;
using System.Diagnostics;
using System.Text.Json;

namespace ShopOnline.Pages.Accounts.Cart
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext dBContext;
        private Dictionary<int, OrderDetail> orderDetailsCard;
        private IHubContext<SignalrServer> signalrServer;

        public IndexModel(PRN221DBContext dBContext, IHubContext<SignalrServer> signalrServer)
        {
            this.dBContext = dBContext;
            this.signalrServer = signalrServer;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public IActionResult OnGet(int? productId)
        {
            Account account = SessionUtils.GetAccountFromSession(HttpContext.Session);
            if (account == null)
            {
                Customer = new Customer();
            }
            else
            {
                Customer = dBContext.Customers.Find(account.CustomerId);
            }
            if (productId == null)
            {
                return Page();
            }
            orderDetailsCard = SessionUtils.GetCartInfo(HttpContext.Session);
            Product productFromDB = dBContext.Products.Find(productId);
            if (productFromDB == null)
            {
                return null;
            }
            OrderDetail orderDetailFromCart = orderDetailsCard[(int)productId];
            if (orderDetailFromCart == null)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Product = productFromDB,
                    ProductId = (int)productId,
                    Quantity = 1,
                    UnitPrice = (decimal)productFromDB.UnitPrice
                };
                orderDetailsCard.Add((int)productId, orderDetail);
            }
            else
            {
                orderDetailFromCart.Quantity++;
            }
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(orderDetailsCard));
            return RedirectToPage("Cart");
        }

        public async Task<IActionResult> OnPost(DateTime? requiredShipDate)
        {
            ModelState.Remove("Customer.CustomerId");
            if (ModelState.IsValid)
            {
                Dictionary<int, OrderDetail> cart = SessionUtils.GetCartInfo(HttpContext.Session);
                if (cart.Count == 0)
                {
                    ViewData["error-message"] = "Cart empty.";
                    return Page();
                }
                if (requiredShipDate <= DateTime.Now)
                {
                    ViewData["error-message"] = "Ship date must be greater than today.";
                    return Page();
                }
                Account account = SessionUtils.GetAccountFromSession(HttpContext.Session);
                string customerId = null;
                if (account == null)
                {
                    customerId = Utils.RandomCustId(dBContext.Customers);
                    Customer.CustomerId = customerId;
                    await dBContext.Customers.AddAsync(Customer);
                }
                if (account != null)
                {
                    customerId = account.CustomerId;
                }

                Order order = new Order
                {
                    OrderDate = DateTime.Now,
                    RequiredDate = requiredShipDate,
                    CustomerId = customerId,
                    ShipAddress = Customer.Address,
                    ShipName = Customer.ContactName,
                };

                order.Freight = cart.Values.Sum(e => e.UnitPrice * e.Quantity);

                await dBContext.Orders.AddAsync(order);
                await dBContext.SaveChangesAsync();

                Order newOrder = dBContext.Orders.OrderByDescending(e => e.OrderId).Take(1).ToList()[0];



                dBContext.Products.Where(e => cart.Keys.Contains(e.ProductId)).ToList().ForEach(
                    e =>
                    {
                        if (e.UnitsOnOrder == null)
                        { e.UnitsOnOrder = cart[e.ProductId].Quantity; }
                        else
                        { e.UnitsOnOrder += cart[e.ProductId].Quantity; }
                    });

                // Remove all product detail from orderDetail in Card
                cart.Values.ToList().ForEach(e => { e.OrderId = newOrder.OrderId; e.Product = null; });



                await dBContext.OrderDetails.AddRangeAsync(cart.Values);


                //orderDetail.ForEach(e =>
                //{
                //   Models.Product product =  dBContext.Products.Find(e.ProductId);
                //   product.UnitsInStock = product.UnitsInStock = e.Quantity;
                //   dBContext.Products.Update(product);
                //});


                HttpContext.Session.Remove("Cart");
                await dBContext.SaveChangesAsync();
                await signalrServer.Clients.All.SendAsync("LoadOrdersHist");
                return Page();
            }
            ViewData["error-message"] = "Provide all required field.";
            return Page();
        }
    }
}
