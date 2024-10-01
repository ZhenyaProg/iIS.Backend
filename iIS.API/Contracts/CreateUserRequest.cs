namespace iIS.API.Contracts
{
    public class CreateUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
    }
}