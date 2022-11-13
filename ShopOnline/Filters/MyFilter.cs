
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
                string tokenFromCookies = context.HttpContext.Request.Cookies[Const.COOKIE_TOKEN].ToString();
                var result = SessionUtils.DecodeJWTTokenGetName(tokenFromCookies);
                Account account = db.Accounts.Find(int.Parse(result[Const.JWT_KEY.SUB]));
                context.HttpContext.Session.SetString(Const.ACCOUNT_SESSION, JsonSerializer.Serialize(account));
            }
            catch (Exception ex)
            {
                //context.Result = new RedirectResult(
                //     "/errorpage?code=401&message=Unautorized"
                //);
                context.HttpContext.Session.Remove(Const.ACCOUNT_SESSION);
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
