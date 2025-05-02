using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ICategoryRepository _categoryRepoFindTitle;
        public CategoryService(IRepository<Category> categoryRepository, ICategoryRepository categoryRepoFindTitle)
        {
            _categoryRepository = categoryRepository;
            _categoryRepoFindTitle = categoryRepoFindTitle;
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
            return await _categoryRepoFindTitle.FindByCategoryTitleAsync(name);
        }

        private Category ConvertCategoryDTOToCAtegoty(CategoryDTO categoryDTO)
        {
            return new Category { CategoryName = categoryDTO.CategoryTitle };
        }
    }
}
