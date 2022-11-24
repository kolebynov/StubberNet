using System.Linq.Expressions;

namespace StubberNet.Core.Objects;

public sealed class GetRepositoryItemsQuery<T>
{
	public Pagination? Pagination { get; init; }

	public IReadOnlyCollection<Expression<Func<T, bool>>>? Filters { get; init; }

	public IReadOnlyCollection<(OrderDirection Direction, Expression<Func<T, object>> KeySelector)>? Orderings { get; init; }
}