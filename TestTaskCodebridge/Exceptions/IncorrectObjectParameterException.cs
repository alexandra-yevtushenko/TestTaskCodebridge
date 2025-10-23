namespace TestTaskCodebridge.Exceptions
{
    [Serializable]
    internal class IncorrectObjectParameterException : Exception
    {
        public IncorrectObjectParameterException()
        {
        }

        public IncorrectObjectParameterException(string? message) : base(message)
        {
        }

        public IncorrectObjectParameterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}