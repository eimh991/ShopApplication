using Microsoft.AspNetCore.Http;
using Moq;
using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Tests.Service
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IProductExtendedRepository> _productExtendedRepoMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _productExtendedRepoMock = new Mock<IProductExtendedRepository>();

            _productService = new ProductService(
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _productExtendedRepoMock.Object
                );

        }

        private IFormFile CreateFakeFormFile()
        {
            var content = "Face image content";
            var filname = "test.jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "Data", filname)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct()
        {
            //Arrange
            var dto = new ProductRequestDTO
            {
                ProductId = 1,
                Name = "Test Product",
                Price = 100,
                Description = "Description",
                Stock = 10,
                CategoryTitle = "Electronics",
                Image = CreateFakeFormFile()
            };

            _categoryRepositoryMock
                .Setup(x => x.FindByCategoryTitleAsync(dto.CategoryTitle, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { CategoryId = 2 });

            //Act
            await _productService.CreateAsync(dto, CancellationToken.None);

            //Assert
            _productRepositoryMock.Verify(x => x.CreateAsync(It.Is<Product>(p => p.Name == dto.Name),
                        It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Act
            await _productService.DeleteAsync(5, CancellationToken.None);

            //Assert
            _productRepositoryMock.Verify(x => x.DeleteAsync(5, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectDto()
        {
            //Arrange
            var product = new Product
            {
                ProductId = 1,
                Name = "Test",
                Price = 50,
                Description = "Description",
                ImagePath = "image.jpg",
                Stock = 5,
                CategoryId = 2,
            };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(product.ProductId, CancellationToken.None);

            //Assert
            Assert.Equal(product.ProductId, result.ProductId);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnDtos()
        {
            //Arrange
            var category = new Category { CategoryId = 3, CategoryName = "animal" };
            var expectedProducts = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "P1",
                    Price = 10,
                    Description = "D",
                    ImagePath = "",
                    Stock = 1,
                    CategoryId = 3
                }
            };

            _categoryRepositoryMock.Setup(x => x.FindByCategoryTitleAsync("animal", It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _productExtendedRepoMock.Setup(x => x.GetAllPaginateAsync(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    category.CategoryId,
                    It.IsAny<CancellationToken>())
                ).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllAsync("search", 10, 1, "asc", "animal", CancellationToken.None);

            // Assert
            Assert.Single(result);
            var dto = result.First();
            Assert.Equal(expectedProducts[0].Name, dto.Name);
            Assert.Equal(expectedProducts[0].ProductId, dto.ProductId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            //Arrange
            var dto = new ProductRequestDTO
            {
                ProductId = 1,
                Name = "Update",
                Price = 150,
                Description = "Update Desc",
                Stock = 10,
                CategoryTitle = "Update Animal",
                Image = CreateFakeFormFile()
            };

            _categoryRepositoryMock
                .Setup(x => x.FindByCategoryTitleAsync(dto.CategoryTitle, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { CategoryId = 1 });

            //Act
            await _productService.UpdateAsync(dto, CancellationToken.None);

            //Assert
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>(p => p.Name == "Update"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangePriceAsync_ShouldCallRepository_WhenPriceIsPositive()
        {
            //Arrange
            var dto = new ProductPriceChangeDTO { ProductId = 1, NewPrice = 200 };

            //Act
            await _productService.ChangePriceAsync(dto, CancellationToken.None);

            //Assert
            _productExtendedRepoMock.Verify(x => x.ChangePriceAsync(It.Is<Product>(p => p.Price == 200),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangeQuantityProductAsync_ShouldCallRepository_WhenStockIsPositive()
        {
            //Arrange
            var dto = new ProductRequestDTO { ProductId = 1, Stock = 5 };

            //Act
            await _productService.ChangeQuantityProductAsync(dto, CancellationToken.None);

            //Assert
            _productExtendedRepoMock.Verify(x => x.ChangeQuantityProductAsync(It.Is<Product>(p => p.Stock == 5),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task GetLastProductsAsync_ShouldReturnProducts()
        {
            //Arrange
            var expectedProducts = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Latest",
                    Price = 10,
                    CategoryId = 1,
                    Description = "Some desc",
                    ImagePath = "image.jpg",
                    Stock = 5
                }
            };

            _productExtendedRepoMock
                .Setup(x => x.GetLastProductsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProducts);

            //Act
            var result = await _productService.GetLastProductsAsync(CancellationToken.None);

            //Assert
            Assert.Single(result);
            var productDto = result.First();
            Assert.Equal(expectedProducts[0].ProductId, productDto.ProductId);
            Assert.Equal(expectedProducts[0].Name, productDto.Name);
        }

        [Fact]
        public async Task ChangeImagePathAsync_ShouldCallRepository()
        {
            //Arrange
            var dto = new ProductRequestChangeImageDTO
            {
                ProductId = 1,
                Image = CreateFakeFormFile()
            };

            //Act
            await _productService.ChangeImagePathAsync(dto, CancellationToken.None);

            //Assert
            _productExtendedRepoMock.Verify(x => x.ChangeImagePathProductAsync(It.Is<Product>(p => p.ProductId == 1),
                It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
