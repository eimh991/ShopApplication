using Shop.DTO;
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
        private readonly IUserAdditionalRepository _userAdditionalRepository;
        public UserService(IRepository<User> userRepository,
                            IPasswordHasher passwordHasher,
                            IJwtProvider jwtProvider,
                            IUserAdditionalRepository userAdditionalRepository)
        {

            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _userAdditionalRepository = userAdditionalRepository;
        }

        public async Task ChangeStatusAsync(int userId,string status, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(status)
                && status.ToLower() == UserRole.Manager.ToString().ToLower()
                || status.ToLower() == UserRole.Admin.ToString().ToLower())
            {

                await _userAdditionalRepository.ChangeStatusAsync(userId,status, cancellationToken);

            }
        }

        public async Task CreateAsync(UserDTO entity , CancellationToken cancellationToken)
        {
            User user = new User()
            {
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = Register(entity.Password),
                Balance = 0.0m,
                UserRole = Enum.UserRole.User,
            };
            
            await  _userRepository.CreateAsync(user, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _userRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllAsync(string search , CancellationToken cancellationToken)
        {
            var users  =  await _userRepository.GetAllAsync(search, cancellationToken);

            return users.Select(u => new UserResponseDTO
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.UserRole.ToString()
            }).ToList();
        }

        public async Task<User> GetByEmaiAsync(string email , CancellationToken cancellationToken)
        {
             return await _userAdditionalRepository.GetByEmailAsync(email, cancellationToken);
        }

        public async Task<UserDTO> GetByIdAsync(int id , CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByIdAsync(id, cancellationToken);

            return ConvertUserToUserDTO(user);
        }

        public async Task UpdateAsync(UserDTO entity , CancellationToken cancellationToken)
        {
            User user = new User()
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = Register(entity.Password),
            };
           await _userRepository.UpdateAsync(user, cancellationToken);
        }

        private string Register(string password)
        {
            return _passwordHasher.Generate(password);  
        }

        public async Task<string> Login(string email ,string password, CancellationToken cancellationToken)
        {
            var user = await _userAdditionalRepository.GetByEmailAsync(email, cancellationToken);
            if (user != null)
            {
                var result = _passwordHasher.Verify(password, user.PasswordHash);
                if (result == false)
                {
                    throw new Exception("Некорректный логин или пароль");
                }

                var token = _jwtProvider.GenerateToken(user);
                return token;
            }

            throw new Exception("Нет такого пользователя");
        }

        public async Task<IEnumerable<CartItem>> GetUserCartItemsAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userAdditionalRepository.GetUserWithCartsItemAsync(userId, cancellationToken);
            if (user != null)
            {
                return user.Cart.CartItems;
            }
            return Enumerable.Empty<CartItem>();
        }

        private UserDTO ConvertUserToUserDTO(User user)
        {
            if (user == null) return null;

            return new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                UserId = user.UserId,
                Balance = user.Balance,
            };
        }

    }
}
