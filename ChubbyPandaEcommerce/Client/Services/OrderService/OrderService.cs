using ChubbyPandaEcommerce.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ChubbyPandaEcommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient http, IAuthService authService, NavigationManager navigationManager)
        {
            _http = http;
            _authService = authService;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/Order/{orderId}");
            return result.Data;
        }

        public async Task<List<OrderOverviewDto>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewDto>>>("api/Order");
            return result.Data;
        }

        public async Task PlaceOrder()
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _http.PostAsync("api/Order", null);
            }
            else
            {
                _navigationManager.NavigateTo("login");
            }
        }
    }
}
