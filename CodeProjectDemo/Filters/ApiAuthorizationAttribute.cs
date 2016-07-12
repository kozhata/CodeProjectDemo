using CodeProjectDemo.Services.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;
using CodeProjectDemo.Models.Exceptions;
using CodeProjectDemo.Models.Token;
using CodeProjectDemo.Models.User;
using System.Web.Http;
using CodeProjectDemo.Models.AppSettings;

namespace CodeProjectDemo.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ApiAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            ITokenService tokenService = filterContext.ControllerContext.Configuration
                .DependencyResolver.GetService(typeof(ITokenService)) as ITokenService;
            IAppSettings appSettings = GlobalConfiguration.Configuration
                .DependencyResolver.GetService(typeof(IAppSettings)) as IAppSettings;

            string apiToken = HttpContext.Current.Request.Headers.Get(appSettings.TokenHeader);

            if (string.IsNullOrWhiteSpace(apiToken))
            {
                apiToken = filterContext.Request.Headers.GetValues(appSettings.TokenHeader).FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(apiToken) && filterContext.Request.Method == HttpMethod.Get)
            {
                apiToken = HttpContext.Current.Request.QueryString["t"];
            }            

            if (string.IsNullOrWhiteSpace(apiToken))
            {
                filterContext.Response = filterContext.Request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, "Suply token");

                return;
            }            

            ApiUser currentUser = tokenService.GetUserByApiToken(apiToken);

            if (currentUser == null)
            {
                filterContext.Response = filterContext.Request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");

                return;
            }

            HttpContext.Current.User = 
                filterContext.ControllerContext.RequestContext.Principal = new CustomPrincipal(currentUser);
        }
    }
}