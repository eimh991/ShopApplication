using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;

namespace Shop.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        public ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task CreateAsync(ProductDTO entity)
        {
            var category =  await ((CategoryRepository)_categoryRepository).FindByCategoryTitlleAsync(entity.CategoryTitle);


            Product product = new Product
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImagePath = сheckingProductPictures(entity.ImagePath),
                Stock = entity.Stock,
                CategoryId = category.CategoryId,
            };
            await _productRepository.CreateAsync(product);


        }

        public async Task DeleteAsync(int id)
        {
           await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductResponceDTO>> GetAllAsync(string search, int paginateSize, int page, string sortOrder)
        {
            var products = await ((ProductRepository)_productRepository).GetAllPaginateAsync(search, paginateSize, page, sortOrder);
            return ConvertProductToProductResponceDTO(products);

        }

        public async Task<ProductResponceDTO> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var responceProduct = ConvertProductToProductResponceDTO(new List<Product> {product})[0];
            return responceProduct;
        }

        public async Task UpdateAsync(ProductDTO entity)
        {
            var category = await ((CategoryRepository)_categoryRepository).FindByCategoryTitlleAsync(entity.CategoryTitle);
            Product product = new Product
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImagePath = сheckingProductPictures(entity.ImagePath),
                Stock = entity.Stock,
                CategoryId = category.CategoryId,
            };
            await _productRepository.UpdateAsync(product);
        }

        public async Task ChangePriceAsync(ProductDTO entity)
        {
            if(entity.Price > 0m)
            {
                Product product = new Product
                {
                    ProductId = entity.ProductId,
                    Price = entity.Price,
                };
                 await ((ProductRepository)_productRepository).ChangePriceAsync(product);
            }
            
        }

        public async Task ChangeQuantityProductAsync(ProductDTO entity)
        {
            if (entity.Stock > 0m)
            {
                Product product = new Product
                {
                    ProductId = entity.ProductId,
                    Stock = entity.Stock,
                };
                await ((ProductRepository)_productRepository).ChangeQuantityProductAsync(product);
            }

        }

        public async Task<IEnumerable<ProductResponceDTO>> GetLastProductsAsync()
        {
            var products = await ((ProductRepository)_productRepository).GetLastProductsAsync();
            return ConvertProductToProductResponceDTO(products);

        }

        private string сheckingProductPictures(string pathImage)
        {
            if (pathImage == string.Empty  || string.IsNullOrWhiteSpace(pathImage))
            {
                pathImage = "/images/default.jpg";
            }
            return pathImage;
        }

        private  List<ProductResponceDTO> ConvertProductToProductResponceDTO(IEnumerable<Product> products)
        {

            return products.Select(p => new ProductResponceDTO()
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImagePath = p.ImagePath,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
            }).ToList();
        }
    }
}
