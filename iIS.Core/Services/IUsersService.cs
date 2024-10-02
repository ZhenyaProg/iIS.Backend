using iIS.Core.Models;

namespace iIS.Core.Services
{
    public interface IUsersService
    {
        Task DeleteUser(Guid userId);
        Task<User> EditUser(Guid userId, string email, DateOnly birthDate);
        Task<User> LogIn(string loginType, string login, string password);
        Task LogOut(Guid userId);
        Task Register(string userName, DateOnly birthDate, string email, string password);
    }
}