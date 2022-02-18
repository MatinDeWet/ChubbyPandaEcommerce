using ChubbyPandaEcommerce.Server.Services.AuthService;
using ChubbyPandaEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChubbyPandaEcommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly IAuthService _authService;

        public CartService(DataContext context, IHttpContextAccessor http, IAuthService authService)
        {
            _context = context;
            _http = http;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();

            var sameItem = await _context.CartItems
                .FirstOrDefaultAsync(
                    ci=>ci.ProductId == cartItem.ProductId && 
                    ci.ProductTypeId == cartItem.ProductTypeId && 
                    ci.UserId == cartItem.UserId
                    );
            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
            };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId, int productTypeId)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(
                    ci => ci.ProductId == productId &&
                    ci.ProductTypeId == productTypeId &&
                    ci.UserId == _authService.GetUserId()
                    );

            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Cart item does not exist."
                };
            }

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
            };
        }

        public async Task<ServiceResponse<int>> GetCartItemCount()
        {
            var count = (await _context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync()).Count();
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductDto>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductDto>>
            {
                Data = new List<CartProductDto>(),
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product == null)
                    continue;

                var productVariant = await _context.ProductVariants
                    .Where(pv=>pv.ProductId == item.ProductId && pv.ProductTypeId == item.ProductTypeId)
                    .Include(pv=>pv.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant == null)
                    continue;

                var cartProduct = new CartProductDto
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeID = productVariant.ProductTypeId,
                    Quantity = item.Quantity
                };
                result.Data.Add(cartProduct);
            }
            return result;
        }

        public async Task<ServiceResponse<List<CartProductDto>>> GetDbCartProducts()
        {
            return await GetCartProductsAsync(await _context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetUserId());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();

            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(
                    ci => ci.ProductId == cartItem.ProductId &&
                    ci.ProductTypeId == cartItem.ProductTypeId &&
                    ci.UserId == cartItem.UserId
                    );

            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Cart item does not exist."
                };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true
            };

        }
    }
}
