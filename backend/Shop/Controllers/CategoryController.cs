using Microsoft.AspNetCore.Mvc;
using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategoriesAsync(string search = "", CancellationToken cancellationToken = default)
        {
            var categories = await _categoryService.GetAllAsync(search, cancellationToken);
            if (categories != null)
            {
                return Ok(categories);
            }
            return NotFound(new { Messge = "По вашему запросу нету категорий" });
        }
        [HttpGet("id")]
        public async Task<ActionResult<Category>> GetCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);
            if (category != null)
            {
                return Ok(category);
            }
            return NotFound(new { Messge = "По вашему запросу категория не найдена" });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CategoryDTO categoryDto, CancellationToken cancellationToken)
        {
            await _categoryService.CreateAsync(categoryDto, cancellationToken);
            return Ok(categoryDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteAsync(categoryId, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> CorrectCategoryAsync(CategoryDTO categoryDto, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateAsync(categoryDto, cancellationToken);
            return Ok(categoryDto);
        }

        [HttpGet("title")]
        public async Task<ActionResult<Category>> GetCategoryByTitle(string title, CancellationToken cancellationToken)
        {
            var category = await ((CategoryService)_categoryService).GetByTitleAsync(title, cancellationToken);
            if (category != null)
            {
                return Ok(category);
            }
            return NotFound(new { Messge = "По вашему запросу категория не найдена" });
        }
    }
}
