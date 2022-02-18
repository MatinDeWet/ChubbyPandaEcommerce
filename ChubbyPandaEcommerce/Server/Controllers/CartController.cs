using ChubbyPandaEcommerce.Server.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChubbyPandaEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet("Count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCount()
        {
            var result = await _cartService.GetCartItemCount();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<int>>> GetDbProducts()
        {
            var result = await _cartService.GetDbCartProducts();
            return Ok(result);
        }

        [HttpPost("Products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductDto>>>>  GetCartProducts([FromBody] List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartProductsAsync(cartItems);
            return Ok(result);
        }

        [HttpPost("Add"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> AddProductItem([FromBody] CartItem cartItem)
        {
            var result = await _cartService.AddToCart(cartItem);
            return Ok(result);
        }

        [HttpPut("UpdateQuantity"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateProductQuantity([FromBody] CartItem cartItem)
        {
            var result = await _cartService.UpdateQuantity(cartItem);
            return Ok(result);
        }

        [HttpDelete("DeleteProduct/{productId}/{productTypeId}"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveProductItem(int productId, int productTypeId)
        {
            var result = await _cartService.DeleteProduct(productId, productTypeId);
            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartProductDto>>>> StoreCartItems([FromBody] List<CartItem> cartItems)
        {
            var result = await _cartService.StoreCartItems(cartItems);
            return Ok(result);
        }
    }
}
