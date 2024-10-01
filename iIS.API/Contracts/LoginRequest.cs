namespace iIS.API.Contracts
{
    public class LoginRequest
    {
        public string LoginType { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}