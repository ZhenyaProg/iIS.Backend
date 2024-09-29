using iIS.Core.Models;

namespace iIS.Core.Repositories
{
    public interface IUsersRepository
    {
        Task Add(User user);
    }
}