﻿using ChubbyPandaEcommerce.Shared;

namespace ChubbyPandaEcommerce.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderOverviewDto>> GetOrders();
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
