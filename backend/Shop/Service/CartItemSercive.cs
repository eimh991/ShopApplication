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
        private readonly IUserAdditionalRepository _additionalRepository;
        private readonly ICartItemCleaner _cartItemCleaner;
        private readonly ICartItemExtendedRepository _cartItemExtendedRepository;

        public CartItemSercive(IRepositoryWithUser<CartItem> cartItemRepository, IRepository<User> userRepository, 
            IRepository<Product> productRepository, IUserAdditionalRepository additionalRepository, 
            ICartItemCleaner cartItemCleaner, ICartItemExtendedRepository cartItemExtendedRepository)
        {
            
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _additionalRepository = additionalRepository;
            _cartItemCleaner = cartItemCleaner;
            _cartItemExtendedRepository = cartItemExtendedRepository;
        }

        public async Task ClearAllCartItemsAsync(int userId)
        {
           await _cartItemCleaner.DeleteAllCartItemsAsync(userId);
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

           var carItemId =  await _cartItemExtendedRepository.AddAsyncAndReturnCartItemId(userId, cartItem);
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

            await _cartItemExtendedRepository.UpdateQuantityAsync(cartItem);
        }

        public async Task<IEnumerable<CartProductDTO>> GetAllCartProductAsync(int userId)
        {
            var cartProduct  = await _cartItemExtendedRepository.GetAllCartProductAsync(userId);

            return cartProduct;
        }

        private async Task<Cart> GetUserCartAsync(int userId) {
            var user = await _additionalRepository.GetUserWithCartAsync(userId);
            if(user != null) {
                return user.Cart;
            }
            throw new Exception(message: "Не удаётся положить товар в корзину");

        }
    }
}
