namespace iIS.API.Contracts
{
    public class EditUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
    }
}