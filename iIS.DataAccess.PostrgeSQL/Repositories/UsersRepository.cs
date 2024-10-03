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

        public async Task<bool> ContainsById(Guid userId)
        {
            var result = await _context.Users.AnyAsync(entity => entity.Id == userId);
            return result;
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
            UserEntity? entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.UserName == login);
            if (entity == null) return null;
            User user = CreateUser(entity);
            return user;
        }

        public async Task<User?> GetByEmail(string email)
        {
            UserEntity? entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Email == email);
            if (entity == null) return null;
            User user = CreateUser(entity);
            return user;
        }

        public async Task<User?> GetById(Guid userId)
        {
            UserEntity? entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id == userId);
            if (entity == null) return null;
            User user = CreateUser(entity);
            return user;
        }

        public async Task Update(Guid userId, User editData)
        {
            await _context.Users
                .Where(userEntity => userEntity.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(entity => entity.UserName, editData.UserName)
                    .SetProperty(entity => entity.Email, editData.Email)
                    .SetProperty(entity => entity.BirthDay, editData.BirthDay.ToShortDateString()));
        }

        public async Task Delete(Guid userId)
        {
            await _context.Users
                .Where(userEntity => userEntity.Id == userId)
                .ExecuteDeleteAsync();
        }

        private User CreateUser(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                UserName = entity.UserName,
                BirthDay = DateOnly.Parse(entity.BirthDay),
                Email = entity.Email,
                HashedPassword = entity.HashedPassword,
                Role = (Role)entity.Role,
            };
        }
    }
}