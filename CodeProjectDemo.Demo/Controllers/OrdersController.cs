using CodeProjectDemo.Demo.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Demo.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : BaseApiController
    {
        [CustomAuthorize(Roles = "IncidentResolvers")]
        [HttpPut]
        [Route("refund/{orderId}")]
        public IHttpActionResult RefundOrder([FromUri]string orderId)
        {
            return Ok();
        }

        [HttpGet]
        [CustomClaimsAuthorize(ClaimType = "FTE", ClaimValue = "1")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}