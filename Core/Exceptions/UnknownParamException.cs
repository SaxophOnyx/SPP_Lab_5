namespace Core.Exceptions
{
    public class UnknownParamException : Exception
    {
        public UnknownParamException(string? message)
            : base(message)
        {

        }

        public UnknownParamException()
            : base()
        {

        }
    }
}
