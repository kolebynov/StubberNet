namespace StubberNet.Core.Models;

public interface IIdentifiable<TId>
	where TId : IEquatable<TId>
{
	public TId Id { get; }
}