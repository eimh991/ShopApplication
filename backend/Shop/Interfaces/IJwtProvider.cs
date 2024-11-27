using Shop.Model;

namespace Shop.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
