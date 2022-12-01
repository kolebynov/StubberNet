using LiteDB;
using StubberNet.Core.Exceptions;
using StubberNet.Core.Models;
using StubberNet.Core.Objects;
using StubberNet.Core.Repository;
using StubberNet.Repository.LiteDb.Extensions;

namespace StubberNet.Repository.LiteDb;

internal sealed class Repository<TModel, TId> : IRepository<TModel, TId>
	where TModel : class, IIdentifiable<TId>
	where TId : IEquatable<TId>
{
	private readonly ILiteCollection<TModel> _liteCollection;
	private readonly BsonMapper _bsonMapper;

	public Repository(ILiteCollection<TModel> liteCollection, BsonMapper bsonMapper)
	{
		_liteCollection = liteCollection ?? throw new ArgumentNullException(nameof(liteCollection));
		_bsonMapper = bsonMapper ?? throw new ArgumentNullException(nameof(bsonMapper));
	}

	public IAsyncEnumerable<TModel> GetItems(GetRepositoryItemsQuery<TModel>? getItemsQuery, CancellationToken cancellationToken) =>
		_liteCollection.Query().ApplyGetItemsQuery(getItemsQuery).ToEnumerable().ToAsyncEnumerable();

	public Task<TModel> GetById(TId id, CancellationToken cancellationToken)
	{
		var result = _liteCollection.FindById(_bsonMapper.Serialize(id));
		if (result == null)
		{
			throw CreateNotFoundException(id);
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
		if (!_liteCollection.Update(item))
		{
			throw CreateNotFoundException(item.Id);
		}

		return Task.CompletedTask;
	}

	public Task Delete(TId id, CancellationToken cancellationToken)
	{
		if (_liteCollection.DeleteMany(x => x.Id.Equals(id)) == 0)
		{
			throw CreateNotFoundException(id);
		}

		return Task.CompletedTask;
	}

	private static ItemNotFoundException CreateNotFoundException(TId itemId) => new($"Item with id {itemId} was not found");
}