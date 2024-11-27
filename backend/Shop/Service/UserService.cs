using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;
using Shop.UsersDTO;

namespace Shop.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IRepository<User> userRepository,
                            IPasswordHasher passwordHasher,
                            IJwtProvider jwtProvider) {

            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task ChangeStatusAsync(int userId,string status)
        {
            if (!string.IsNullOrEmpty(status)
                && status.ToLower() == UserRole.Manager.ToString().ToLower()
                && status.ToLower() == UserRole.Admin.ToString().ToLower())
            {

                await ((UserRepository)_userRepository).ChangeStatusAsync(userId,status);

            }
        }

        public async Task CreateAsync(UserDTO entity)
        {
            User user = new User()
            {
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = Register(entity.Password),
                Balance = 0.0m,
                UserRole = Enum.UserRole.User,
            };
            
            await  _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync(string search)
        {
            return await _userRepository.GetAllAsync(search);
        }

        public async Task<User> GetByEmaiAsync(string email)
        {
             return await ((UserRepository)_userRepository).GetByEmailAsync(email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(UserDTO entity)
        {
            User user = new User()
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = Register(entity.Password),
            };
           await _userRepository.UpdateAsync(user);
        }

        private string Register(string password)
        {
            return _passwordHasher.Generate(password);  
        }

        public async Task<string> Login (string email ,string password)
        {
            var user = await ((UserRepository)_userRepository).GetByEmailAsync(email);
            if (user != null)
            {
                var result = _passwordHasher.Verify(password, user.PasswordHash);
                if (result == false)
                {
                    throw new Exception("Некоректный логин или пароль");
                }

                var token = _jwtProvider.GenerateToken(user);
                return token;
            }

            throw new Exception("Нет такого пользователя");
        }

        public async Task<IEnumerable<CartItem>> GetUserCartItemsAsync(int userId)
        {
            var user = await ((UserRepository)_userRepository).GetUserWithCartsItemAsync(userId);
            if (user != null)
            {
                return user.Cart.CartItems;
            }
            return Enumerable.Empty<CartItem>();
        }

    }
}
