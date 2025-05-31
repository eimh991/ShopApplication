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
        public async Task<IActionResult> CreateProductAsync([FromForm]ProductRequestDTO productDTO, CancellationToken cancellationToken)
        {
            await _productService.CreateAsync(productDTO, cancellationToken);

            return Ok();
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductResponceDTO>> GetProductByIdAsync(int productID, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(productID, cancellationToken);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound(new { Message = "Такого продукта нету" });
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResponceDTO>>> GetProductsAsync(string search = "", 
            int paginateSize = 9, int page = 1, string sortOrder = "", string categoryName = "", CancellationToken cancellationToken = default)
        {
            var products = await _productService.GetAllAsync(search, paginateSize,page,sortOrder, categoryName,cancellationToken);
            if (!products.Any())
            {
                return NotFound(new { Message = "Нету продуктов с таким названием или описанием" });
            }
            return Ok(products);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int productId, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(productId, cancellationToken);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductAsync(ProductRequestDTO productDTO, CancellationToken cancellationToken)
        {
            await _productService.UpdateAsync(productDTO, cancellationToken);

            return Ok();
        }

        [HttpPut("price")]
        public async Task<IActionResult> ChangePriceProduct([FromBody] ProductPriceChangeDTO productDTO, CancellationToken cancellationToken)
        {
            await _productService.ChangePriceAsync(productDTO, cancellationToken);

            return Ok();
        }

        [HttpGet("last")]
        public async Task<ActionResult<List<ProductResponceDTO>>> GetLastProductsAsync(CancellationToken cancellationToken)
        {
            var products = await ((ProductService)_productService).GetLastProductsAsync(cancellationToken);
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound(new { Messge = "Нету продуктов с таким названием или описанием" });
        }

        
        [HttpPut("imagePath")]
        [AuthorizeRole(UserRole.Admin)]
        public async Task<IActionResult> ChangeImagePathProduct([FromForm] ProductRequestChangeImageDTO productDTO, CancellationToken cancellationToken)
        {
            await ((ProductService)_productService).ChangeImagePathAsync(productDTO, cancellationToken);

            return Ok();
        }
    }
}
