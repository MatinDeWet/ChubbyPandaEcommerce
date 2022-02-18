using ChubbyPandaEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ChubbyPandaEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategoriesAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategoriesAsync();


        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategoriesAsync(int id)
        {
            Category category = await GetCategoryById(id);
            if (category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "No category found"
                };
            }

            category.Deleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }



        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            var categories = await _context.Categories
            .Where(c => !c.Deleted)
            .ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c=> !c.Deleted && c.Visible)
                .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategoriesAsync(Category category)
        {
            Category Dbcategory = await GetCategoryById(category.Id);
            if (Dbcategory == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "No category found"
                };
            }

            Dbcategory.Name = category.Name;
            Dbcategory.Url = category.Url;
            Dbcategory.Visible = category.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        private async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
