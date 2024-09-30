namespace iIS.Core.Errors
{
    public class ExistUserException : Exception
    {
        public ExistUserException(string? message = null) : base(message)
        {
            
        }
    }
}