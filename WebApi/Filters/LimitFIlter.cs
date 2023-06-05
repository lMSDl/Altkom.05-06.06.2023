using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class LimitFilter : IAsyncActionFilter
    {
        private int _limitPerMinute;
        private int _counter;

        public LimitFilter(int limitPerMinute)
        {
            _limitPerMinute = limitPerMinute;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(_counter >= _limitPerMinute)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            Interlocked.Increment(ref _counter);

            await next();

            _ = Task.Delay(60000).ContinueWith(x => Interlocked.Decrement(ref _counter));

        }
    }
}
