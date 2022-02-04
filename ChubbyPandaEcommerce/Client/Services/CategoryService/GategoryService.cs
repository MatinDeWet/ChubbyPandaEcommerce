using ChubbyPandaEcommerce.Shared;
using System.Net.Http.Json;

namespace ChubbyPandaEcommerce.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;
        public CategoryService(HttpClient http)
        {
            _http = http;
        }
        public List<Category> categories { get; set; } = new List<Category>();

        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");
            if (response != null && response.Data != null)
                categories = response.Data;
        }
    }
}
