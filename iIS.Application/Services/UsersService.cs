using iIS.Core.Repositories;
using iIS.Core.Services;

namespace iIS.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public Task Register(string userName, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}