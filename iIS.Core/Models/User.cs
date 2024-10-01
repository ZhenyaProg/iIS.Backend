using iIS.Core.Auth;

namespace iIS.Core.Models
{
    public class User
    {
        public User(Guid id, string userName, DateOnly birthDay, string email, string hashedPassword, Role role)
        {
            Id = id;
            UserName = userName;
            BirthDay = birthDay;
            Email = email;
            HashedPassword = hashedPassword;
            Role = role;
        }

        public Guid Id { get; }
        public string UserName { get; }
        public DateOnly BirthDay { get; }
        public string Email { get; }
        public string HashedPassword { get; }
        public Role Role { get; }
    }
}