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

        public async Task ClearAllCartItemsAsync(int userId, CancellationToken cancellationToken)
        {
           await _cartItemCleaner.DeleteAllCartItemsAsync(userId, cancellationToken);
        }

        public async Task<int> CreateCartItemAsync(CartItemDTO cartItemDTO, CancellationToken cancellationToken)
        {
            var userId = cartItemDTO.UserId;
            var product = await _productRepository.GetByIdAsync(cartItemDTO.ProductId, cancellationToken);

            var cartItem = new CartItem()
            {
                Cart = await GetUserCartAsync(userId, cancellationToken),
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

           var carItemId =  await _cartItemExtendedRepository.AddAsyncAndReturnCartItemId(userId, cartItem, cancellationToken);
            return carItemId;
        }

        public async Task DeleteCartItemAsync(int cartItemId, CancellationToken cancellationToken)
        {
            await _cartItemRepository.DeleteAsync(cartItemId, cancellationToken);
        }

        public async Task<IEnumerable<CardItemResponseDTO>> GetAllCartItemsAsync(int userId, CancellationToken cancellationToken)
        {
            var items =  await _cartItemRepository.GetAllAsync(userId, cancellationToken);
            return items.Select(i=> new CardItemResponseDTO()
            {
                CartId = i.CartId,
                Quantity = i.Quantity,
                ProductId = i.ProductId,
                CartItemId = i.CartItemId,
            }).ToList();
        }

         

        public async Task<CartItem> GetCartItemByIdAsync(int userId, int entityId, CancellationToken cancellationToken)
        {
            return await _cartItemRepository.GetByIdAsync(userId, entityId, cancellationToken);
        }

        public async Task UpdateCountCartItemsAsync(int cartItemId, int quantity, CancellationToken cancellationToken)
        {
            var cartItem = new CartItem { CartItemId = cartItemId, Quantity = quantity };

            await _cartItemExtendedRepository.UpdateQuantityAsync(cartItem, cancellationToken);
        }

        public async Task<IEnumerable<CartProductDTO>> GetAllCartProductAsync(int userId, CancellationToken cancellationToken)
        {
            var cartProduct  = await _cartItemExtendedRepository.GetAllCartProductAsync(userId, cancellationToken);

            return cartProduct;
        }

        private async Task<Cart> GetUserCartAsync(int userId, CancellationToken cancellationToken) 
        {
            var user = await _additionalRepository.GetUserWithCartAsync(userId, cancellationToken);
            if(user != null) {
                return user.Cart;
            }
            throw new Exception(message: "Не удаётся положить товар в корзину");

        }
    }
}
