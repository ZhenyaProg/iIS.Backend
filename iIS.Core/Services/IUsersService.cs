
namespace iIS.Core.Services
{
    public interface IUsersService
    {
        Task Register(string userName, string email, string password);
    }
}