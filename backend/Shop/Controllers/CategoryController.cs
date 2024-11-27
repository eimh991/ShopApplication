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
        public async Task<ActionResult<List<Category>>> GetAllCategories(string search = "")
        {
            var categories = await _categoryService.GetAllAsync(search);
            if (categories != null)
            {
                return Ok(categories);
            }
            return NotFound(new { Messge = "По вашему запросу нету категорий" });
        }
        [HttpGet("id")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category != null)
            {
                return Ok(category);
            }
            return NotFound(new { Messge = "По вашему запросу категория не найдена" });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDto)
        {
            await _categoryService.CreateAsync(categoryDto);
            return Ok(categoryDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> CorrectCategory(CategoryDTO categoryDto)
        {
            await _categoryService.UpdateAsync(categoryDto);
            return Ok(categoryDto);
        }

        [HttpGet("title")]
        public async Task<ActionResult<Category>> GetCategoryByTitle(string title)
        {
            var category = await ((CategoryService)_categoryService).GetByTitleAsync(title);
            if (category != null)
            {
                return Ok(category);
            }
            return NotFound(new { Messge = "По вашему запросу категория не найдена" });
        }
    }
}
