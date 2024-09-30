using iIS.Core.Models;

namespace iIS.Core.Repositories
{
    public interface IUsersRepository
    {
        Task Add(User user);
        Task<bool> ContainsByEmail(string email);
        Task<bool> ContainsByName(string userName);
    }
}