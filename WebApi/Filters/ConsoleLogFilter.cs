using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class ConsoleLogFilter : IActionFilter
    {



        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Begin of " + context.HttpContext.Request.Method);
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("End of " + context.HttpContext.Request.Method);
        }
    }
}
