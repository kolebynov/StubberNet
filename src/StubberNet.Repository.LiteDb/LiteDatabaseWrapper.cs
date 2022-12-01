using LiteDB;
using LiteDB.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace StubberNet.Repository.LiteDb;

internal sealed class LiteDatabaseWrapper : ILiteDatabase
{
	private readonly ILiteDatabase _innerLiteDatabase;
	private readonly IServiceProvider _serviceProvider;

	public BsonMapper Mapper => _innerLiteDatabase.Mapper;

	public ILiteStorage<string> FileStorage => _innerLiteDatabase.FileStorage;

	public int UserVersion
	{
		get => _innerLiteDatabase.UserVersion;
		set => _innerLiteDatabase.UserVersion = value;
	}

	public TimeSpan Timeout
	{
		get => _innerLiteDatabase.Timeout;
		set => _innerLiteDatabase.Timeout = value;
	}

	public bool UtcDate
	{
		get => _innerLiteDatabase.UtcDate;
		set => _innerLiteDatabase.UtcDate = value;
	}

	public long LimitSize
	{
		get => _innerLiteDatabase.LimitSize;
		set => _innerLiteDatabase.LimitSize = value;
	}

	public int CheckpointSize
	{
		get => _innerLiteDatabase.CheckpointSize;
		set => _innerLiteDatabase.CheckpointSize = value;
	}

	public Collation Collation => _innerLiteDatabase.Collation;

	public LiteDatabaseWrapper(ILiteDatabase innerLiteDatabase, IServiceProvider serviceProvider)
	{
		_innerLiteDatabase = innerLiteDatabase ?? throw new ArgumentNullException(nameof(innerLiteDatabase));
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
	}

	public void Dispose() => _innerLiteDatabase.Dispose();

	public ILiteCollection<T> GetCollection<T>(string name, BsonAutoId autoId = BsonAutoId.ObjectId) =>
		ConfigureCollection(() => _innerLiteDatabase.GetCollection<T>(name, autoId));

	public ILiteCollection<T> GetCollection<T>() => ConfigureCollection(() => _innerLiteDatabase.GetCollection<T>());

	public ILiteCollection<T> GetCollection<T>(BsonAutoId autoId) =>
		ConfigureCollection(() => _innerLiteDatabase.GetCollection<T>(autoId));

	public ILiteCollection<BsonDocument> GetCollection(string name, BsonAutoId autoId = BsonAutoId.ObjectId) =>
		ConfigureCollection(() => _innerLiteDatabase.GetCollection(name, autoId));

	public bool BeginTrans() => _innerLiteDatabase.BeginTrans();

	public bool Commit() => _innerLiteDatabase.Commit();

	public bool Rollback() => _innerLiteDatabase.Rollback();

	public ILiteStorage<TFileId> GetStorage<TFileId>(string filesCollection = "_files", string chunksCollection = "_chunks") =>
		_innerLiteDatabase.GetStorage<TFileId>(filesCollection, chunksCollection);

	public IEnumerable<string> GetCollectionNames() => _innerLiteDatabase.GetCollectionNames();

	public bool CollectionExists(string name) => _innerLiteDatabase.CollectionExists(name);

	public bool DropCollection(string name) => _innerLiteDatabase.DropCollection(name);

	public bool RenameCollection(string oldName, string newName) => _innerLiteDatabase.RenameCollection(oldName, newName);

	public IBsonDataReader Execute(TextReader commandReader, BsonDocument? parameters = null) =>
		_innerLiteDatabase.Execute(commandReader, parameters);

	public IBsonDataReader Execute(string command, BsonDocument? parameters = null) => _innerLiteDatabase.Execute(command, parameters);

	public IBsonDataReader Execute(string command, params BsonValue[] args) => _innerLiteDatabase.Execute(command, args);

	public void Checkpoint() => _innerLiteDatabase.Checkpoint();

	public long Rebuild(RebuildOptions? options = null) => _innerLiteDatabase.Rebuild(options);

	public BsonValue Pragma(string name) => _innerLiteDatabase.Pragma(name);

	public BsonValue Pragma(string name, BsonValue value) => _innerLiteDatabase.Pragma(name, value);

	private ILiteCollection<T> ConfigureCollection<T>(Func<ILiteCollection<T>> collectionProvider)
	{
		var collection = collectionProvider();
		foreach (var collectionConfigurator in _serviceProvider.GetServices<ILiteCollectionConfigurator<T>>())
		{
			collectionConfigurator.Configure(collection);
		}

		return collection;
	}
}