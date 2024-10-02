using iIS.Core.Models;

namespace iIS.Core.Auth
{
    public interface ITokenProvider
    {
        public string GenerateToken(User user);
    }
}