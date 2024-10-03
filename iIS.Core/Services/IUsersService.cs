using iIS.Core.Models;

namespace iIS.Core.Services
{
    public interface IUsersService
    {
        Task DeleteUser(Guid userId);
        Task<User> EditUser(Guid userId, User editData);
        Task<User> LogIn(string loginType, string login, string password);
        Task LogOut(Guid userId);
        Task Register(User user, string password);
    }
}