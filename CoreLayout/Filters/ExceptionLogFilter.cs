using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NLog;
using NLog.Web;
using System;

namespace CoreLayout.Filters
{
    public class ExceptionLogFilter : IExceptionFilter
    {
        public ExceptionLogFilter()
        {

        }

        public void OnException(ExceptionContext filterContext)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            string traceId = Guid.NewGuid().ToString();
            LogManager.Configuration.Variables["traceId"] = traceId;
            logger.Error(filterContext.Exception, filterContext.Exception.Message);
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                            { "controller", "Home" },
                                            { "action", "ExceptionLog" },
                                            { "traceId", traceId}
                                        });
        }
    }
}
