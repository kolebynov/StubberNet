using LiteDB;

namespace StubberNet.Infrastructure.LiteDb.Abstractions;

internal interface ILiteCollectionConfigurator<T>
{
	void Configure(ILiteCollection<T> liteCollection);
}