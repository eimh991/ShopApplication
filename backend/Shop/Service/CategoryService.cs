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
        public async Task CreateAsync(CategoryDTO entity, CancellationToken cancellationToken)
        {
            await _categoryRepository.CreateAsync(ConvertCategoryDTOToCAtegoty(entity),cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
           await _categoryRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string search, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllAsync(search, cancellationToken);
        }

        public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task UpdateAsync(CategoryDTO entity, CancellationToken cancellationToken)
        {
            return _categoryRepository.UpdateAsync(ConvertCategoryDTOToCAtegoty(entity), cancellationToken);
        }

        public async Task<Category> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            return await _categoryRepoFindTitle.FindByCategoryTitleAsync(name, cancellationToken);
        }

        private Category ConvertCategoryDTOToCAtegoty(CategoryDTO categoryDTO)
        {
            return new Category { CategoryName = categoryDTO.CategoryTitle };
        }
    }
}
