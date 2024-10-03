using iIS.Core.Auth;

namespace iIS.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateOnly BirthDay { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}