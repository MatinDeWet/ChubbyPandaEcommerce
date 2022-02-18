﻿using ChubbyPandaEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ChubbyPandaEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                .Include(p=>p.Variants)
                .ThenInclude(v=>v.ProductType)
                .FirstOrDefaultAsync(p=>p.Id == productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "No product found";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                    .Include(p => p.Variants)
                    .ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductListAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Include(p => p.Variants)
                    .ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<List<string>>> ProductSearchSuggestions(string searchText)
        {
            var products = await FindProductBySearchText(searchText);
            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    result.Add(product.Title);

                if (!string.IsNullOrWhiteSpace(product.Description))
                {
                    var punctiation = product.Description
                                .Where(char.IsPunctuation)
                                .Distinct()
                                .ToArray();
                    var words = product.Description
                                .Split()
                                .Select(s => s.Trim(punctiation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText,StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                            result.Add(word);
                    }
                }
            }

            return new ServiceResponse<List<string>>
            {
                Data = result
            };
        }

        public async Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductBySearchText(searchText)).Count / pageResults);
            var products = await _context.Products
                                .Where(p =>
                                    p.Title.ToLower().Contains(searchText.ToLower()) ||
                                    p.Description.ToLower().Contains(searchText.ToLower())
                                    )
                                .Include(p => p.Variants)
                                .Skip((page-1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();

            var response = new ServiceResponse<ProductSearchResultDto>
            {
                Data = new ProductSearchResultDto
                {
                    products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };
            return response;
        }
        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Featured == true)
                    .Include(p => p.Variants)
                    .ToListAsync()
            };
            return response;
        }

        private async Task<List<Product>> FindProductBySearchText(string searchText)
        {
            return await _context.Products
                                .Where(p =>
                                    p.Title.ToLower().Contains(searchText.ToLower()) ||
                                    p.Description.ToLower().Contains(searchText.ToLower())
                                    )
                                .Include(p => p.Variants)
                                .ToListAsync();
        }
    }
}
