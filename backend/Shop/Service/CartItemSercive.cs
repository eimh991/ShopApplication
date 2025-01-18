using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;

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

        public async Task CreateCartItemAsync(CartItemDTO cartItemDTO)
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

            await _cartItemRepository.AddAsync(userId, cartItem);
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteAsync(cartItemId);
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsAsync(int userId)
        {
            return await _cartItemRepository.GetAllAsync(userId);
        }

        public async Task<CartItem> GetCartItemByIdAsync(int userId, int entityId)
        {
            return await _cartItemRepository.GetByIdAsync(userId, entityId);
        }

        public async Task UpdateCountCartItemsAsync(int userId, int cartItemId, int quentity)
        {
            var cartItem = new CartItem { CartItemId = cartItemId, Quantity = quentity };

            await _cartItemRepository.UpdateAsync(userId, cartItem);
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
