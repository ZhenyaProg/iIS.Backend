using iIS.Core.Auth;
using iIS.Core.Errors;
using iIS.Core.Models;
using iIS.Core.Repositories;
using iIS.Core.Services;

namespace iIS.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Register(string userName, DateOnly birthDate, string email, string password)
        {
            bool containsUser = await _usersRepository.ContainsByName(userName) &&
                                await _usersRepository.ContainsByEmail(email);
            if (containsUser)
                throw new ExistUserException("Пользователь с таким ником или email`ом уже существует");

            User newUser = new User(Guid.NewGuid(), userName, birthDate, email, _passwordHasher.Hash(password), Role.User);
            await _usersRepository.Add(newUser);
        }

        public async Task<User> Login(string loginType, string login, string password)
        {
            User? user = loginType switch
            {
                "un" => await _usersRepository.GetByName(login),
                "email" => await _usersRepository.GetByEmail(login),
                _ => throw new ArgumentException(loginType)
            };

            if (user is null)
                throw new NotFoundUserException("Неверный логин");

            if (!_passwordHasher.Verify(password, user.HashedPassword))
                throw new WrongPasswordException("Неверный пароль");

            return user;
        }
    }
}