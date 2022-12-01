using LiteDB;

namespace StubberNet.Repository.LiteDb;

internal interface ILiteCollectionConfigurator<T>
{
	void Configure(ILiteCollection<T> liteCollection);
}