using LiteDB;
using MassTransit;
using StubberNet.Core.Exceptions;
using StubberNet.Core.Models.Dynamic;
using StubberNet.Core.Repository;

namespace StubberNet.Repository.LiteDb;

internal sealed class ResourceDataRepository : IResourceDataRepository
{
	private readonly ILiteDatabase _liteDatabase;

	public ResourceDataRepository(ILiteDatabase liteDatabase)
	{
		_liteDatabase = liteDatabase ?? throw new ArgumentNullException(nameof(liteDatabase));
	}

	public IAsyncEnumerable<Token> GetResourceData(NewId resourceId, CancellationToken cancellationToken)
	{
		var collectionName = GetCollectionName(resourceId);
		if (!_liteDatabase.CollectionExists(collectionName))
		{
			throw new ItemNotFoundException($"Resource data was not found for resource {resourceId}");
		}

		return _liteDatabase.GetCollection(collectionName).FindAll()
			.Select(x => _liteDatabase.Mapper.Deserialize<Token>(x["value"])).ToAsyncEnumerable();
	}

	public Task SetResourceData(NewId resourceId, IEnumerable<Token> data, CancellationToken cancellationToken)
	{
		var collection = _liteDatabase.GetCollection(GetCollectionName(resourceId));
		collection.DeleteAll();
		collection.InsertBulk(data.Select(x => new BsonDocument { { "value", _liteDatabase.Mapper.Serialize(x) } }));

		return Task.CompletedTask;
	}

	private static string GetCollectionName(NewId resourceId) => $"resourceData_{resourceId:N}";
}