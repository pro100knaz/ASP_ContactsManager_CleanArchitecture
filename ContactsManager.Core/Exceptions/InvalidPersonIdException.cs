namespace Exceptions
{
	public class InvalidPersonIdException : Exception
	{
		public InvalidPersonIdException()
		{
		}

		public InvalidPersonIdException(string? message) : base(message)
		{
		}

		public InvalidPersonIdException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}
