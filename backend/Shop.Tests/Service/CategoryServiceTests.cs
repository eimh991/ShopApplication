
using Moq;
using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using System.ComponentModel;

namespace Shop.Tests.Service
{
    public class CategoryServiceTests
    {
        private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepoFindTitleMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<IRepository<Category>>();
            _categoryRepoFindTitleMock = new Mock<ICategoryRepository>();

            _categoryService = new CategoryService(
                _categoryRepositoryMock.Object,
                _categoryRepoFindTitleMock.Object
                );
        }

        [Fact]
        public async Task CreateAsycn_ShouldCallRepositoryWithConvertedEntity()
        {
            //Arrange
            var dto = new CategoryDTO { CategoryTitle = "Books" };

            //Act
            await _categoryService.CreateAsync(dto);

            //Assert
            _categoryRepositoryMock.Verify(r => r.CreateAsync(It.Is<Category>(c => c.CategoryName == dto.CategoryTitle)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteById()
        {
            //Arrange
            int categoryId = 1;

            //Act
            await _categoryService.DeleteAsync(categoryId);

            //Assert
            _categoryRepositoryMock.Verify(r=>r.DeleteAsync(categoryId), Times.Once);   
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCategories()
        {
            //Arrange
            string categoryName = "tech";
            var categories = new List<Category>() { 
                new Category() { CategoryName = categoryName } 
            };

            _categoryRepositoryMock.Setup(r=>r.GetAllAsync(categoryName))
                .ReturnsAsync(categories);

            //Act
            var result =  await _categoryService.GetAllAsync(categoryName);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(categoryName, result.First().CategoryName);
        }
        
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCategory()
        {
            //Act
            int categoryId = 2;
            var category = new Category() { CategoryId = categoryId, CategoryName = "Fashion" };

            _categoryRepositoryMock.Setup(r=>r.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            //Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            //Assert
            Assert.Equal(categoryId,result.CategoryId);
            Assert.Equal("Fashion", result.CategoryName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdateWithConvertedEntity()
        {
            //Arrange
            var dto = new CategoryDTO { CategoryTitle = "UpdatedTitle" };

            //Act
            await _categoryService.UpdateAsync(dto);

            //Assert
            _categoryRepositoryMock.Verify(r=>r.UpdateAsync(
                It.Is<Category>(c=>c.CategoryName == "UpdatedTitle")), Times.Once());   
        }

        [Fact]
        public async Task GetByTitleAsync_ShouldReturnCategoryByTitle()
        {
            //Arrange
            string title = "Gadgets";
            var category = new Category { CategoryName = title };

            _categoryRepoFindTitleMock.Setup(r=>r.FindByCategoryTitleAsync(title))
                .ReturnsAsync(category);
            //Act
            var result = await _categoryService.GetByTitleAsync(title);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(title,result.CategoryName);

        }
    }

    
}
