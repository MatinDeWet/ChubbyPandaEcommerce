using ChubbyPandaEcommerce.Shared;

namespace ChubbyPandaEcommerce.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        event Action OnChange;

        List<Category> Admincategories { get; set; }
        List<Category> categories { get; set; }
        Task GetCategories();
        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);

        Category CreateNewCategory();
    }
}
