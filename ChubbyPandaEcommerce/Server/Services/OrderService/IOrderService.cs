namespace ChubbyPandaEcommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders();
        Task<ServiceResponse<OrderDetailsDto>> GetOrdersDetails(int orderId);
    }
}
