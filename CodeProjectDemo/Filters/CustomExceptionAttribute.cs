using CodeProjectDemo.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace CodeProjectDemo.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (actionExecutedContext.Exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (actionExecutedContext.Exception is ForbiddenException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            
            if (statusCode == HttpStatusCode.InternalServerError)
            {
#if DEBUG
                throw new HttpResponseException(actionExecutedContext.Request.CreateErrorResponse(statusCode, actionExecutedContext.Exception));
#else
                bool isTaskCancel = actionExecutedContext.Exception is OperationCanceledException;
                if (isTaskCancel == false)
                {
                    ILogService logService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogService)) as ILogService;
                    if (logService != null)
                    {
                        logService.LogException(actionExecutedContext.Exception);
                    }
                }
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                       resultStatusCode, "There has been internal server error, please try again later or contact support");
#endif
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                       statusCode, statusCode.ToString());
            }            
        }
    }
}