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
        public string Message { get; set; } = "Loading Products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

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

            CurrentPage = 1;
            PageCount = 0;
            if (products.Count == 0)
            {
                Message = "No Products found";
            }

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> ProductSearchSuggestion(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/SearchSuggestion/{searchText}");
            return result.Data;
        }

        public async Task SearchProduct(string searchText, int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResultDto>>($"api/Product/Search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                LastSearchText = searchText;
                products = result.Data.products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
                

            if (products.Count == 0)
                Message = "No products found";

            ProductsChanged?.Invoke();
        }
    }
}
