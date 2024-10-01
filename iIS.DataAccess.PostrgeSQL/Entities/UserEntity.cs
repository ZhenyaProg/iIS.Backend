namespace iIS.DataAccess.PostrgeSQL.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string BirthDay { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public byte Role { get; set; }
    }
}