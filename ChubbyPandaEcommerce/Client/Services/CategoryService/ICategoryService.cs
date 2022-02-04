using ChubbyPandaEcommerce.Shared;

namespace ChubbyPandaEcommerce.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> categories { get; set; }
        Task GetCategories();
    }
}
