using ChubbyPandaEcommerce.Shared;
using System.Net.Http.Json;

namespace ChubbyPandaEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> products { get; set; } = new List<Product>();

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = string.IsNullOrWhiteSpace(categoryUrl) ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/Category/{categoryUrl}");

            if (result != null && result.Data != null)
                products = result.Data;

            ProductsChanged.Invoke();
        }
    }
}
