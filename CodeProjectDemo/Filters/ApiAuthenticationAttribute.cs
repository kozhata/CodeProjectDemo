using CodeProjectDemo.Models.AppSettings;
using CodeProjectDemo.Models.User;
using CodeProjectDemo.Services.Account;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CodeProjectDemo.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ApiAuthenticationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            BasicUser identity = FetchAuthHeader(filterContext);

            if (identity == null)
            {
                filterContext.Response = filterContext.Request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, "Suply Basic Auth");

                return;
            }

            CustomPrincipal genericPrincipal = new CustomPrincipal(identity);

            HttpContext.Current.User = filterContext.ControllerContext.RequestContext.Principal = genericPrincipal;

            bool isAuthenticated = AuthenticateUser(identity.Name, identity.Password, filterContext);

            if (!isAuthenticated)
            {
                filterContext.Response = filterContext.Request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");

                return;
            }
        }

        private static bool AuthenticateUser(string username, string password, HttpActionContext actionContext)
        {
            IAccountService accountService = GlobalConfiguration.Configuration
                .DependencyResolver.GetService(typeof(IAccountService)) as IAccountService;

            if (accountService != null)
            {
                UserDto dbUser = accountService.Authenticate(username, password);

                if (dbUser != null)
                {
                    BasicUser basicAuthenticationIdentity =
                        HttpContext.Current.User.Identity as BasicUser;

                    if (basicAuthenticationIdentity != null)
                    {
                        basicAuthenticationIdentity.Id = dbUser.Id;
                    }

                    return true;
                }
            }

            return false;
        }

        private static BasicUser FetchAuthHeader(HttpActionContext filterContext)
        {
            IAppSettings appSettings = GlobalConfiguration.Configuration
                .DependencyResolver.GetService(typeof(IAppSettings)) as IAppSettings;

            string authHeaderValue = HttpContext.Current.Request.Headers.Get(appSettings.AuthorizationHeader);

            if (string.IsNullOrWhiteSpace(authHeaderValue))
            {
                authHeaderValue = filterContext.Request.Headers.GetValues(appSettings.AuthorizationHeader).First();
            }

            if (string.IsNullOrEmpty(authHeaderValue))
            {
                return null;
            }

            string scheme = authHeaderValue.Split(' ').FirstOrDefault();

            if (string.IsNullOrEmpty(scheme) || scheme != "Basic")
            {
                return null;
            }

            authHeaderValue = authHeaderValue.Split(' ').LastOrDefault();

            if (string.IsNullOrEmpty(authHeaderValue))
            {
                return null;
            }

            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));

            string[] credentials = authHeaderValue.Split(':');

            return credentials.Length < 2 ? null : new BasicUser
            {
                UserName = credentials[0],
                Password = credentials[1]
            };
        }
    }
}