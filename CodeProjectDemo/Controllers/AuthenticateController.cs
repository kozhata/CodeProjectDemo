using CodeProjectDemo.Filters;
using CodeProjectDemo.Models.Token;
using CodeProjectDemo.Models.User;
using CodeProjectDemo.Services.Token;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeProjectDemo.Controllers
{
    [ApiAuthentication]
    [RoutePrefix("v1/authenticate")]
    public class AuthenticateController : ApiController
    {
        private readonly ITokenService _tokenService;

        public AuthenticateController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null
                && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                BasicUser basicUser =
                    System.Threading.Thread.CurrentPrincipal.Identity as BasicUser;

                if (basicUser != null)
                {
                    int userId = basicUser.Id;

                    TokenDto token = await _tokenService.GenerateToken(userId);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");

                    response.Headers.Add("Token", token.AuthToken);
                    response.Headers.Add("TokenExpiry", "60min");
                    response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");

                    return response;
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error");
        }
    }
}