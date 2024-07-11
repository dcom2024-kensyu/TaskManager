using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Web;
using System.Security.Claims;
using System.Text.Json;

namespace TaskManager.Web.Filter
{
    public class AccessLogFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            OutputAccessLog(context, "Start");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            OutputAccessLog(context, "End");
        }

        private void OutputAccessLog(FilterContext context, string starOrtEnd)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var controllerActionDescripter = context.ActionDescriptor as ControllerActionDescriptor;
                var name = context.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                logger.Info($"Controller:{controllerActionDescripter.ControllerName} " +
                                $"Action:{controllerActionDescripter.ActionName} " +
                                $"User:{(name ?? "Not User")} {starOrtEnd}");
            }
            catch (Exception ex)
            {
                logger.Error($"\r\n ログ出力時にエラーが発生しました。{ex}");
            }
        }
    }
}