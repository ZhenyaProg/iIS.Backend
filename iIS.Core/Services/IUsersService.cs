using iIS.Core.Models;

namespace iIS.Core.Services
{
    public interface IUsersService
    {
        Task<User> Login(string loginType, string login, string password);
        Task Register(string userName, DateOnly birthDate, string email, string password);
    }
}