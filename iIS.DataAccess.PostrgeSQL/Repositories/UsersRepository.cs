using iIS.Core.Models;
using iIS.Core.Repositories;
using iIS.DataAccess.PostrgeSQL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        private User CreateUser(UserEntity entity)
        {
            return new User(entity.Id, entity.UserName, entity.Email, entity.HashedPassword, (Role)entity.Role);
        }
    }
}