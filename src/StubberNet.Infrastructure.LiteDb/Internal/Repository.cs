using System.Linq.Expressions;
using LiteDB;
using StubberNet.Core.Abstractions;
using StubberNet.Core.Exceptions;
using StubberNet.Core.Objects;
using StubberNet.Infrastructure.LiteDb.Extensions;

namespace StubberNet.Infrastructure.LiteDb.Internal;

internal sealed class Repository<TModel, TId> : IRepository<TModel, TId>
	where TModel : class, IIdentifiable<TId>
	where TId : IEquatable<TId>
{
	private readonly ILiteCollection<TModel> _liteCollection;

	public Repository(ILiteCollection<TModel> liteCollection)
	{
		_liteCollection = liteCollection ?? throw new ArgumentNullException(nameof(liteCollection));
	}

	public Task<IReadOnlyCollection<TModel>> GetItems(GetRepositoryItemsQuery<TModel>? getItemsQuery, CancellationToken cancellationToken)
	{
		var items = _liteCollection.Query()
			.ApplyFilters(getItemsQuery?.Filters)
			.ApplyOrderings(getItemsQuery?.Orderings)
			.ApplyPagination(getItemsQuery?.Pagination)
			.ToArray();

		return Task.FromResult<IReadOnlyCollection<TModel>>(items);
	}

	public Task<TModel> GetById(TId id)
	{
		var result = _liteCollection.FindOne(x => x.Id.Equals(id));
		if (result == null)
		{
			throw new ItemNotFoundException($"Item with id {id} was not found");
		}

		return Task.FromResult(result);
	}

	public Task Add(TModel item, CancellationToken cancellationToken)
	{
		_liteCollection.Insert(item);

		return Task.CompletedTask;
	}

	public Task Update(TModel item, CancellationToken cancellationToken)
	{
		_liteCollection.Update(item);

		return Task.CompletedTask;
	}

	public Task Delete(TModel item, CancellationToken cancellationToken)
	{
		_liteCollection.DeleteMany(x => x.Id.Equals(item.Id));

		return Task.CompletedTask;
	}
}