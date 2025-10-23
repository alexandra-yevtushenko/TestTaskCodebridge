namespace TestTaskCodebridge.Exceptions
{
    [Serializable]
    internal class IncorrectQueryArgumentException : Exception
    {
        public IncorrectQueryArgumentException()
        {
        }

        public IncorrectQueryArgumentException(string? message) : base(message)
        {
        }

        public IncorrectQueryArgumentException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}