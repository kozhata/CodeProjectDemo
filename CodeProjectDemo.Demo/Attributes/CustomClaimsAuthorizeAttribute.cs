using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CodeProjectDemo.Demo.Attributes
{
    public class CustomClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return base.OnAuthorizationAsync(actionContext, cancellationToken);
            }

            if (!(principal.HasClaim(x => x.Type == ClaimType && x.Value == ClaimValue)))
            //if (!(principal.HasClaim(ClaimType, ClaimValue)))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return base.OnAuthorizationAsync(actionContext, cancellationToken);
            }

            //User is Authorized, complete execution
            //return Task.FromResult<object>(null);
            return base.OnAuthorizationAsync(actionContext, cancellationToken);            
        }
    }
}