using ChubbyPandaEcommerce.Shared;

namespace ChubbyPandaEcommerce.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductDto>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeID);
        Task UpdateQuantity(CartProductDto product);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemCount();

    }
}
