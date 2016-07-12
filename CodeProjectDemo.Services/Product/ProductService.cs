namespace CodeProjectDemo.Services.Product
{
    using System;
    using System.Linq;
    using Models.Product;
    using DataModel.UnitOfWork;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using Models.Exceptions;
    using Models.User;
    using Models.Token;
    public class ProductService : IProductService
    {
        private readonly ApiUser _apiUser;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork, ApiUser apiUser)
        {
            _unitOfWork = unitOfWork;
            _apiUser = apiUser;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productModel)
        {
            DataModel.Product dbProduct = new DataModel.Product
            {
                Name = productModel.Name
            };

            _unitOfWork.Products.Insert(dbProduct);
            await _unitOfWork.SaveAsync();

            return productModel;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            DataModel.Product dbProduct = await _unitOfWork.Products.All().
                FirstOrDefaultAsync(p => p.Id == productId);

            if (dbProduct == null)
            {
                throw new NotFoundException(); 
            }

            _unitOfWork.Products.Delete(dbProduct);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public IQueryable<ProductDto> GetAllProducts()
        {
            //if (!_apiUser.Roles.HasFlag(RolesEnum.LevelThree))
            //{
            //    throw new ForbiddenException();
            //}
            IQueryable<ProductDto> queryProducts = _unitOfWork.Products.All()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedOn = p.CreatedOn
                });

            return queryProducts;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            ProductDto productDto = await _unitOfWork.Products.All()
                .Where(p => p.Id == productId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .FirstOrDefaultAsync();

            if (productDto == null)
            {
                throw new NotFoundException();
            }

            return productDto;
        }

        public async Task<bool> UpdateProduct(ProductDto productModel)
        {
            DataModel.Product dbProduct = await _unitOfWork.Products.All()
                .FirstOrDefaultAsync(p => p.Id == productModel.Id);

            dbProduct.Name = productModel.Name;

            _unitOfWork.Products.Update(dbProduct);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
