using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopOnline.Common;

namespace ShopOnline.Filters
{
    public class AdminFilter : IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            //throw new NotImplementedException();
            if (!SessionUtils.isAdminSession(context.HttpContext.Session))
            {
                context.Result = new RedirectResult(
                     "/errorpage?code=401&message=Unautorized"
                );
            }
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
