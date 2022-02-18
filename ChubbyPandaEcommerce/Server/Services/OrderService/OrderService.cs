using ChubbyPandaEcommerce.Server.Services.CartService;
using ChubbyPandaEcommerce.Server.Services.AuthService;
using ChubbyPandaEcommerce.Server.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ChubbyPandaEcommerce.Server.Services.OrderService
{
    public class OrderService: IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _http;

        public OrderService(DataContext dataContext, ICartService cartService, IHttpContextAccessor http, IAuthService authService)
        {
            _context = dataContext;
            _cartService = cartService;
            _http = http;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewDto>>();
            var orders = await _context.Orders
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .Where(o => o.UserId == _authService.GetUserId())
                            .OrderByDescending(o => o.OrderDate)
                            .ToListAsync();

            var orderResponse = new List<OrderOverviewDto>();
            orders.ForEach(o => orderResponse.Add(
                new OrderOverviewDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count-1} more...": o.OrderItems.First().Product.Title,
                    ProductImageUrl = o.OrderItems.First().Product.ImageUrl,
                    total = o.TotalPrice
                }));

            response.Data = orderResponse;
            return response;
        }

        public async Task<ServiceResponse<OrderDetailsDto>> GetOrdersDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsDto>();

            var order = await _context.Orders
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.ProductType)
                            .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                            .OrderByDescending(o => o.OrderDate)
                            .FirstOrDefaultAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found";
                return response;
            }

            var orderDetailDto = new OrderDetailsDto
            {
                OrderDate = order.OrderDate,
                TotaslPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductDto>()
            };

            order.OrderItems.ForEach(oi =>
                        orderDetailDto.Products.Add(new OrderDetailsProductDto
                        {
                            ProductId = oi.ProductId,
                            ImageUrl = oi.Product.ImageUrl,
                            ProductType = oi.ProductType.Name,
                            Quantity = oi.Quantity,
                            Title = oi.Product.Title,
                            totalPrice = oi.TotalPrice,

                        }));

            response.Data = orderDetailDto;
            return response;


        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;

            //decimal totalPrice = 0;
            //products.ForEach(p => totalPrice+=p.Price*p.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(p => orderItems.Add(new OrderItem
            {
                ProductId = p.ProductId,
                ProductTypeId = p.ProductTypeID,
                Quantity = p.Quantity,
                TotalPrice = p.Price * p.Quantity
            }));

            var order = new Order 
            { 
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItems,
                TotalPrice= orderItems.Sum(oi=> oi.TotalPrice)
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(_context.CartItems.Where(ci=>ci.UserId == _authService.GetUserId()));
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true
            };
        }

    }
}
