using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Demo.Controllers
{
    [Authorize]
    [RoutePrefix("api/claims")]
    public class ClaimsController : BaseApiController
    {        
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetClaims()
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            return Ok(claims);
        }
    }
}