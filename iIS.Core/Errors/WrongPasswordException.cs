namespace iIS.Core.Errors
{
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException(string? message = null) : base(message)
        {
            
        }
    }
}