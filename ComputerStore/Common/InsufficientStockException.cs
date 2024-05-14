namespace API.Common
{
public class InsufficientStockException : Exception
    {
        public InsufficientStockException() { }

        public InsufficientStockException(string message) : base(message) { }

        public InsufficientStockException(string message, Exception innerException) : base(message, innerException) { }
    }

}
