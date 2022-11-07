using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NPOI.SS.UserModel;
using ShopOnline.Common;
using ShopOnline.Models;
using ShopOnline.SignalRLab;
using Syncfusion.XlsIO;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace ShopOnline.Pages.Accounts.Cart
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext dBContext;
        private IWebHostEnvironment env;
        private Dictionary<int, OrderDetail> orderDetailsCard;
        private IHubContext<SignalrServer> signalrServer;
        private IConverter converter;

        public IndexModel(PRN221DBContext dBContext, IHubContext<SignalrServer> signalrServer, IConverter converter, IWebHostEnvironment env)
        {
            this.dBContext = dBContext;
            this.signalrServer = signalrServer;
            this.converter = converter;
            this.env = env;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public IActionResult OnGet()
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
            orderDetailsCard = SessionUtils.GetCartInfo(HttpContext.Session);

            ViewData["totalAmount"] = orderDetailsCard.Values.Sum(e => e.UnitPrice * e.Quantity);
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(orderDetailsCard));
            return Page();
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
                    Customer.CreateDate = DateTime.Now;
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

                ViewData["error-message"] = "Order success";
                HttpContext.Session.Remove("Cart");
                await dBContext.SaveChangesAsync();
                await signalrServer.Clients.All.SendAsync("LoadOrdersHist");
                if (account != null)
                {
                    var file = GeneratePdfInvoice(order);
                    string body = new HTMLTemplate().MailConfrimOrder(order);
                    await Utils.Email(account.Email, "Mail Invoice", body, new System.Net.Mail.Attachment(new MemoryStream(file), "Invoice.pdf"));
                }
                return Page();
            }
            ViewData["error-message"] = "Provide all required field.";
            return Page();
        }

        private byte[] GeneratePdfInvoice(Order order)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = Utils.GetHTMLOrderString(order),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "pdfassets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            return converter.Convert(pdf);

            //return File(file, "application/pdf", "Invoice.pdf");
        }


    }
}
