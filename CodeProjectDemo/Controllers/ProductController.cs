namespace CodeProjectDemo.Controllers
{
    using Filters;
    using Helpers;
    using Models.Exceptions;
    using Models.Product;
    using Models.RestBase;
    using Models.Token;
    using Models.User;
    using Services.Product;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.OData.Query;
    
    [RoutePrefix("v1/products")]
    [ApiAuthorization]
    public class ProductController : ApiController
    {
        private readonly ApiUser _apiUser;
        private readonly IProductService _productservice;

        public ProductController(IProductService productService, ApiUser apiUser)
        {
            _productservice = productService;
            _apiUser = apiUser;
        }

        [HttpGet]
        [Route("")]
        public async Task<PagedResponse<ProductDto>> GetAllProducts(ODataQueryOptions<ProductDto> query)
        {
            return await _productservice.GetAllProducts().ApplyQueryAsync(query);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ProductDto> GetProduct(int Id)
        {
            return await _productservice.GetProductById(Id);
        }
    }
}