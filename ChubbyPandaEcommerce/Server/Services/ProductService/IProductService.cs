namespace ChubbyPandaEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductListAsync();
        Task<ServiceResponse<List<Product>>> GetProductByCategory(string categoryUrl);
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
        Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string searchText,int page);
        Task<ServiceResponse<List<string>>> ProductSearchSuggestions(string searchText);
        Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
    }
}
