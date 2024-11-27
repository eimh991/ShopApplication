using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;

namespace Shop.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task CreateAsync(CategoryDTO entity)
        {
            await _categoryRepository.CreateAsync(ConvertCategoryDTOToCAtegoty(entity));
        }

        public async Task DeleteAsync(int id)
        {
           await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string search)
        {
            return await _categoryRepository.GetAllAsync(search);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public Task UpdateAsync(CategoryDTO entity)
        {
            return _categoryRepository.UpdateAsync(ConvertCategoryDTOToCAtegoty(entity));
        }

        public async Task<Category> GetByTitleAsync(string name)
        {
            return await ((CategoryRepository)_categoryRepository).FindByCategoryTitlleAsync(name);
        }

        private Category ConvertCategoryDTOToCAtegoty(CategoryDTO categoryDTO)
        {
            return new Category { CategoryName = categoryDTO.CategoryTitle };
        }
    }
}
