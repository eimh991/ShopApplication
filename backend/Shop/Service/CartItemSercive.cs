using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;
using System.Xml.Linq;

namespace Shop.Service
{
    public class CartItemSercive : ICartItemService
    {
        private readonly IRepositoryWithUser<CartItem> _cartItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;

        public CartItemSercive(IRepositoryWithUser<CartItem> cartItemRepository, IRepository<User> userRepository, IRepository<Product> productRepository)
        {
            
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task ClearAllCartItemsAsync(int userId)
        {
           await ((CartItemRepository)_cartItemRepository).DeleteAllCartItemsAsync(userId);
        }

        public async Task<int> CreateCartItemAsync(CartItemDTO cartItemDTO)
        {
            var userId = cartItemDTO.UserId;
            var product = await _productRepository.GetByIdAsync(cartItemDTO.ProductId);

            var cartItem = new CartItem()
            {
                Cart = await GetUserCartAsync(userId),
                Quantity = cartItemDTO.Quantity,
                Product = product
                /*
                Product = new Product()
                {
                        ProductId = cartItemDTO.ProductId,
                        Description = cartItemDTO.Description,
                        CategoryId = cartItemDTO.CategoryId,
                        ImagePath = cartItemDTO.ImagePath,
                        Name = cartItemDTO.Name,
                        Price = cartItemDTO.Price,
                },*/
            };

           var carItemId =  await ((CartItemRepository)_cartItemRepository).AddAsyncAndReturnCartItemId(userId, cartItem);
            return carItemId;
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteAsync(cartItemId);
        }

        public async Task<IEnumerable<CardItemResponseDTO>> GetAllCartItemsAsync(int userId)
        {
            var items =  await _cartItemRepository.GetAllAsync(userId);
            return items.Select(i=> new CardItemResponseDTO()
            {
                CartId = i.CartId,
                Quantity = i.Quantity,
                ProductId = i.ProductId,
                CartItemId = i.CartItemId,
            }).ToList();
        }

         

        public async Task<CartItem> GetCartItemByIdAsync(int userId, int entityId)
        {
            return await _cartItemRepository.GetByIdAsync(userId, entityId);
        }

        public async Task UpdateCountCartItemsAsync(int cartItemId, int quantity)
        {
            var cartItem = new CartItem { CartItemId = cartItemId, Quantity = quantity };

            await ((CartItemRepository)_cartItemRepository).UpdateAsync(cartItem);
        }

        public async Task<IEnumerable<CartProductDTO>> GetAllCartProductAsync(int userId)
        {
            var cartProduct  = await ((CartItemRepository)_cartItemRepository).GetAllCartProductAsync(userId);

            return cartProduct;
        }

        private async Task<Cart> GetUserCartAsync(int userId) {
            var user = await ((UserRepository)_userRepository).GetUserWithCartAsync(userId);
            if(user != null) {
                return user.Cart;
            }
            throw new Exception(message: "Не удаеться положить товар в корхину");

        }
    }
}
