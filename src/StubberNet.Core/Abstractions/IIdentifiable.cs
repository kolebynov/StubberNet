namespace StubberNet.Core.Abstractions;

public interface IIdentifiable<TId>
	where TId : IEquatable<TId>
{
	public TId Id { get; }
}