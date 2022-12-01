using MassTransit;
using StubberNet.Core.Models.Dynamic;

namespace StubberNet.Core.Repository;

internal interface IResourceDataRepository
{
	IAsyncEnumerable<Token> GetResourceData(NewId resourceId, CancellationToken cancellationToken);

	Task SetResourceData(NewId resourceId, IEnumerable<Token> data, CancellationToken cancellationToken);
}