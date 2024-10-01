using iIS.Core.Auth;
using iIS.Core.Models;
using iIS.Core.Repositories;
using iIS.DataAccess.PostrgeSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace iIS.DataAccess.PostrgeSQL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext _context;

        public UsersRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            UserEntity entity = new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                BirthDay = user.BirthDay.ToShortDateString(),
                Email = user.Email,
                HashedPassword = user.HashedPassword,
                Role = (byte)user.Role
            };

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ContainsByName(string userName)
        {
            var result = await _context.Users.AnyAsync(entity => entity.UserName == userName);
            return result;
        }

        public async Task<bool> ContainsByEmail(string email)
        {
            var result = await _context.Users.AnyAsync(entity => entity.Email == email);
            return result;
        }

        public async Task<User?> GetByName(string login)
        {
            UserEntity? entity = await _context.Users.FirstOrDefaultAsync(entity => entity.UserName == login);
            if (entity == null) return null;
            User user = CreateUser(entity);
            return user;
        }

        public async Task<User?> GetByEmail(string email)
        {
            UserEntity? entity = await _context.Users.FirstOrDefaultAsync(entity => entity.Email == email);
            if (entity == null) return null;
            User user = CreateUser(entity);
            return user;
        }

        private User CreateUser(UserEntity entity)
        {
            return new User(entity.Id, entity.UserName, DateOnly.Parse(entity.BirthDay), entity.Email, entity.HashedPassword, (Role)entity.Role);
        }
    }
}