using System.Linq.Expressions;
using LiteDB;
using StubberNet.Core.Objects;

namespace StubberNet.Infrastructure.LiteDb.Extensions;

internal static class LiteDbExtensions
{
	public static ILiteQueryable<TModel> ApplyFilters<TModel>(
		this ILiteQueryable<TModel> query, IReadOnlyCollection<Expression<Func<TModel, bool>>>? filters)
	{
		foreach (var filter in filters ?? Array.Empty<Expression<Func<TModel, bool>>>())
		{
			query = query.Where(filter);
		}

		return query;
	}

	public static ILiteQueryable<TModel> ApplyOrderings<TModel>(
		this ILiteQueryable<TModel> query, IReadOnlyCollection<(OrderDirection, Expression<Func<TModel, object>>)>? orderings)
	{
		foreach (var (direction, keySelector) in orderings ?? Array.Empty<(OrderDirection, Expression<Func<TModel, object>>)>())
		{
			query = direction == OrderDirection.Ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
		}

		return query;
	}

	public static ILiteQueryableResult<TModel> ApplyPagination<TModel>(this ILiteQueryableResult<TModel> query, Pagination? pagination)
	{
		if (pagination is not { Skip: var skip, Top: var top })
		{
			return query;
		}

		query = skip > 0 ? query.Skip(skip) : query;
		query = top > 0 ? query.Limit(top) : query;

		return query;
	}
}