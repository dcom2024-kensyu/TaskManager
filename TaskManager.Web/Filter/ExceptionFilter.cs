using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Web;
using System.Security.Claims;

namespace TaskManager.Web.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var name = context.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                logger.Error(
                    $"Controller:{controllerActionDescriptor.ControllerName} " +
                    $"Action:{controllerActionDescriptor.ActionName} " +
                    $"User:{(name ?? "No User")} " +
                    "予期せぬ例外が発生しました。" + Environment.NewLine +
                    "************************************************" + Environment.NewLine +
                    $"{context.Exception}" + Environment.NewLine +
                    "************************************************");
            }
            catch (Exception ex)
            {
                logger.Error($"\r\n ログ出力時にエラーが発生しました。{ex}");
            }
        }
    }
}
