namespace iIS.Core.Models
{
    public class User
    {
        public User(Guid id, string userName, string email, string hashedPassword, Role role)
        {
            Id = id;
            UserName = userName;
            Email = email;
            HashedPassword = hashedPassword;
            Role = role;
        }

        public Guid Id { get; }
        public string UserName { get; }
        public string Email { get; }
        public string HashedPassword { get; }
        public Role Role { get; }
    }
}