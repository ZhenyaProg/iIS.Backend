using iIS.Core.Models;

namespace iIS.Core.Repositories
{
    public interface IUsersRepository
    {
        Task Add(User user);
        Task<bool> ContainsByName(string userName);
        Task<bool> ContainsByEmail(string userName);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByName(string login);
    }
}