using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopOnline.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomPageFilterAttribute : Attribute, IAsyncPageFilter
    {
        // Executes first
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            // TODO: implement this
        }

        // Executes last
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            // Before action execution

            await next();

            // After action execution
        }
    }
}
