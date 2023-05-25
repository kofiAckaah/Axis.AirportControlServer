using AircraftAPI.Infrastructure.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.ConfigurationOptions;

namespace AircraftAPI.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VerifyPublicKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(GlobalConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new ContentResult { Content = "401"};

                return;
            }
            
            var config = context.HttpContext.RequestServices.GetService<IOptions<AppConfiguration>>().Value.ApiPublicKey;

            if (config == null || !config.Equals(extractedApiKey.ToList().First()))
            {
                context.Result = new ContentResult() { Content = "401"};
                return;
            }

            await next();
        }
    }
}
