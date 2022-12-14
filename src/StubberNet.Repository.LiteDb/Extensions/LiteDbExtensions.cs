using System.Linq.Expressions;
using LiteDB;
using StubberNet.Core.Objects;

namespace StubberNet.Repository.LiteDb.Extensions;

internal static class LiteDbExtensions
{
	public static ILiteQueryable<TModel> ApplyGetItemsQuery<TModel>(
		this ILiteQueryable<TModel> query, GetRepositoryItemsQuery<TModel>? getItemsQuery) =>
		query
			.ApplyFilters(getItemsQuery?.Filters)
			.ApplyOrderings(getItemsQuery?.Orderings)
			.ApplyPagination(getItemsQuery?.Pagination);

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

	public static ILiteQueryable<TModel> ApplyPagination<TModel>(this ILiteQueryable<TModel> query, Pagination? pagination)
	{
		if (pagination is not { Skip: var skip, Top: var top })
		{
			return query;
		}

		query = skip > 0 ? (ILiteQueryable<TModel>)query.Skip(skip) : query;
		query = top > 0 ? (ILiteQueryable<TModel>)query.Limit(top) : query;

		return query;
	}
}