namespace StubberNet.Core.Exceptions;

public sealed class ItemNotFoundException : Exception
{
	public ItemNotFoundException()
	{
	}

	public ItemNotFoundException(string? message)
		: base(message)
	{
	}

	public ItemNotFoundException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}
}