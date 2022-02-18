using Blazored.LocalStorage;
using ChubbyPandaEcommerce.Shared;
using System.Net.Http.Json;

namespace ChubbyPandaEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IAuthService _authService;

        public CartService(ILocalStorageService localStorage, HttpClient http, IAuthService authService)
        {
            _localStorage = localStorage;
            _http = http;
            _authService = authService;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _http.PostAsJsonAsync("api/Cart/Add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(i => i.ProductId == cartItem.ProductId && i.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }

                await _localStorage.SetItemAsync("cart", cart);
            }

            await GetCartItemCount();
        }

        public async Task GetCartItemCount()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/Cart/Count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemCount", cart != null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }

        public async Task<List<CartProductDto>> GetCartProducts()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductDto>>>("api/Cart/");
                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                    return new List<CartProductDto>();

                var response = await _http.PostAsJsonAsync("api/Cart/Products", cartItems);
                var CartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductDto>>>();
                return CartProducts.Data;
            } 
        }

        public async Task RemoveProductFromCart(int productId, int productTypeID)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _http.DeleteAsync($"api/Cart/DeleteProduct/{productId}/{productTypeID}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                    return;

                var cartItem = cart.Find(i => i.ProductId == productId && i.ProductTypeId == productTypeID);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
            //await GetCartItemCount();
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
                return;

            await _http.PostAsJsonAsync("api/Cart/", cart);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductDto product)
        {
            if (await _authService.IsUserAuthenticated())
            {
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    ProductTypeId = product.ProductTypeID,
                    Quantity = product.Quantity,
                };
                await _http.PutAsJsonAsync("api/Cart/UpdateQuantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                    return;

                var cartItem = cart.Find(i => i.ProductId == product.ProductId && i.ProductTypeId == product.ProductTypeID);
                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }


    }
}
