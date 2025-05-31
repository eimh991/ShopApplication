using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;

namespace Shop.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductExtendedRepository _productExtendedRepository;
        public ProductService(IRepository<Product> productRepository, ICategoryRepository categoryRepo, IProductExtendedRepository productExtendedRepository)
        {
            _productRepository = productRepository;
            _categoryRepo = categoryRepo;
            _productExtendedRepository = productExtendedRepository;
        }
        public async Task CreateAsync(ProductRequestDTO entity, CancellationToken cancellationToken)
        {
            var category =  await _categoryRepo.FindByCategoryTitleAsync(entity.CategoryTitle, cancellationToken);

            string imagePaath = GenerateImagePath(entity.Image).Result;

            Product product = new Product
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImagePath = CheckingProductPictures(imagePaath),
                Stock = entity.Stock,
                CategoryId = category.CategoryId,
            };
            await _productRepository.CreateAsync(product, cancellationToken);


        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
           await _productRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ProductResponceDTO>> GetAllAsync(string search, int paginateSize, 
            int page, string sortOrder, string categoryName, CancellationToken cancellationToken)
        {
           var categoryId = await GetCategoryIdByCategoryNameAsync(categoryName,cancellationToken);
            var products = await _productExtendedRepository.GetAllPaginateAsync(search, paginateSize, 
                    page, sortOrder,categoryId,cancellationToken);
            return ConvertProductToProductResponceDTO(products);

        }

        public async Task<ProductResponceDTO> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            var responceProduct = ConvertProductToProductResponceDTO(new List<Product> {product})[0];
            return responceProduct;
        }

        public async Task UpdateAsync(ProductRequestDTO entity, CancellationToken cancellationToken)
        {
            var category = await _categoryRepo.FindByCategoryTitleAsync(entity.CategoryTitle,cancellationToken);
            string imagePaath = GenerateImagePath(entity.Image).Result;
            Product product = new Product
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImagePath = CheckingProductPictures(imagePaath),
                Stock = entity.Stock,
                CategoryId = category.CategoryId,
            };
            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        public async Task ChangePriceAsync(ProductPriceChangeDTO entity, CancellationToken cancellationToken)
        {
            if(entity.NewPrice > 0m)
            {
                Product product = new Product
                {
                    ProductId = entity.ProductId,
                    Price = entity.NewPrice,
                };

                if (product == null)
                    throw new Exception("Продукт не найден");

                await _productExtendedRepository.ChangePriceAsync(product, cancellationToken);
            }
            
        }

        public async Task ChangeQuantityProductAsync(ProductRequestDTO entity, CancellationToken cancellationToken)
        {
            if (entity.Stock > 0m)
            {
                Product product = new Product
                {
                    ProductId = entity.ProductId,
                    Stock = entity.Stock,
                };
                await _productExtendedRepository.ChangeQuantityProductAsync(product, cancellationToken);
            }

        }

        public async Task<IEnumerable<ProductResponceDTO>> GetLastProductsAsync(CancellationToken cancellationToken)
        {
            var products = await _productExtendedRepository.GetLastProductsAsync(cancellationToken);
            return ConvertProductToProductResponceDTO(products);

        }

        public async Task ChangeImagePathAsync(ProductRequestChangeImageDTO entity, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                ProductId = entity.ProductId,
                ImagePath = CheckingProductPictures(GenerateImagePath(entity.Image).Result),
            };
            await _productExtendedRepository.ChangeImagePathProductAsync(product, cancellationToken);

        }

        private string CheckingProductPictures(string pathImage)
        {
            if (pathImage == string.Empty  || string.IsNullOrWhiteSpace(pathImage))
            {
                pathImage = "default.jpg";
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

        private async Task<string> GenerateImagePath (IFormFile file)
        {
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "images");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }
            var guid = Guid.NewGuid();
            if (file != null && file.Length > 0)
            {
                var fp = file.FileName.Split('.');
                var imageFileName = guid.ToString() + "." + fp[fp.Length - 1];
                var filePath = Path.Combine(uploadFolderPath, imageFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return imageFileName;
            }
            return string.Empty;
        }

        private async Task<int> GetCategoryIdByCategoryNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            int categoryId = 0;
            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = await _categoryRepo.FindByCategoryTitleAsync(categoryName, cancellationToken);
                if (category != null)
                {
                    categoryId = category.CategoryId;
                }
            }
            return categoryId;
        }
    }
}
