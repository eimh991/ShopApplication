using Microsoft.AspNetCore.Mvc;
using Shop.Attribute;
using Shop.DTO;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using Shop.UsersDTO;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromForm]ProductRequestDTO productDTO)
        {
            await _productService.CreateAsync(productDTO);

            return Ok();
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductResponceDTO>> GetProductByIdAsync(int productID)
        {
            var product = await _productService.GetByIdAsync(productID);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound(new { Message = "Такого продукта нету" });
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResponceDTO>>> GetProductsAsync(string search = "", 
            int paginateSize = 9, int page = 1, string sortOrder = "", string categoryName = "")
        {
            var products = await _productService.GetAllAsync(search, paginateSize,page,sortOrder, categoryName);
            if (!products.Any())
            {
                return NotFound(new { Message = "Нету продуктов с таким названием или описанием" });
            }
            return Ok(products);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            await _productService.DeleteAsync(productId);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductAsync(ProductRequestDTO productDTO)
        {
            await _productService.UpdateAsync(productDTO);

            return Ok();
        }

        [HttpPut("price")]
        public async Task<IActionResult> ChangePriceProduct([FromForm]ProductRequestDTO productDTO)
        {
            await ((ProductService)_productService).ChangePriceAsync(productDTO);

            return Ok();
        }

        [HttpGet("last")]
        public async Task<ActionResult<List<ProductResponceDTO>>> GetLastProductsAsync()
        {
            var products = await ((ProductService)_productService).GetLastProductsAsync();
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound(new { Messge = "Нету продуктов с таким названием или описанием" });
        }

        
        [HttpPut("imagePath")]
        [AuthorizeRole(UserRole.Admin)]
        public async Task<IActionResult> ChangeImagePathProduct([FromForm] ProductRequestChangeImageDTO productDTO)
        {
            await ((ProductService)_productService).ChangeImagePathAsync(productDTO);

            return Ok();
        }
    }
}
