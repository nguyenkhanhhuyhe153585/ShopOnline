
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NPOI.OpenXmlFormats.Spreadsheet;
using NuGet.Protocol;
using ShopOnline.Common;
using ShopOnline.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Xml.Serialization;

namespace ShopOnline.Filters
{
    public class MyFilter : IPageFilter
    {
        private readonly PRN221DBContext db;
        public MyFilter(PRN221DBContext _db)
        {
            db = _db;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {


        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)

        {

            try
            {
                string tokenFromCookies = context.HttpContext.Request.Cookies["Token"].ToString();
                var result = SessionUtils.DecodeJWTTokenGetName(tokenFromCookies);
                Account account = db.Accounts.Find(int.Parse(result["Sub"]));
                context.HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(account));
            }
            catch (Exception ex)
            {
                //context.Result = new RedirectToRouteResult(
                //     "/errorpage?code=401&message=Unautorized"
                //); 
            }
            //throw new NotImplementedException();
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
