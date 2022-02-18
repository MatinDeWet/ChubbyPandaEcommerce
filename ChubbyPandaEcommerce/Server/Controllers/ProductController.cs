using ChubbyPandaEcommerce.Server.Data;
using ChubbyPandaEcommerce.Server.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChubbyPandaEcommerce.Server.Controllers
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

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var result = await _productService.GetProductListAsync();
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);
        }

        [HttpGet("Category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductByCategory(string categoryUrl)
        {
            var result = await _productService.GetProductByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet("Search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchText, int page = 1)
        {
            var result = await _productService.SearchProducts(searchText,page);
            return Ok(result);
        }

        [HttpGet("SearchSuggestion/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> ProductSearchSuggestion(string searchText)
        {
            var result = await _productService.ProductSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("Featured")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProducts();
            return Ok(result);
        }
    }
}
