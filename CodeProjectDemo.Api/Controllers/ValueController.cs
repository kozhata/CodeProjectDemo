using CodeProjectDemo.Services.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeProjectDemo.Api.Controllers
{
    [RoutePrefix("api")]
    public class ValueController : ApiController
    {
        private readonly IProductService _service;

        public ValueController(IProductService service)
        {
            _service = service;
        }

        [Route("ok")]
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.GetAllProducts().ToList());
        }
    }
}
