namespace CodeProjectDemo.Services.Product
{
    using System.Linq;
    using Models.Product;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<ProductDto> GetProductById(int productId);

        IQueryable<ProductDto> GetAllProducts();

        Task<ProductDto> CreateProduct(ProductDto productEntity);

        Task<bool> UpdateProduct(ProductDto productEntity);

        Task<bool> DeleteProduct(int productId);
    }
}
