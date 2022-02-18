namespace ChubbyPandaEcommerce.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
        Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();
        Task<ServiceResponse<List<Category>>> AddCategoriesAsync(Category category);
        Task<ServiceResponse<List<Category>>> UpdateCategoriesAsync(Category category);
        Task<ServiceResponse<List<Category>>> DeleteCategoriesAsync(int id);
    }
}
