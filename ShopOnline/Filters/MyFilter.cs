
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol;
using ShopOnline.Common;
using System.IdentityModel.Tokens.Jwt;

namespace ShopOnline.Filters
{
    public class MyFilter : IPageFilter
    {
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            string tokenFromCookies = context.HttpContext.Request.Cookies["Token"].ToString();
            string result = SessionUtils.DecodeJWTTokenGetName(tokenFromCookies).ToJson();
            Console.WriteLine("Hehehehe " + result);
            ;
            //throw new NotImplementedException();
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
