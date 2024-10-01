namespace iIS.Core.Errors
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(string? message = null) : base(message)
        {

        }
    }
}