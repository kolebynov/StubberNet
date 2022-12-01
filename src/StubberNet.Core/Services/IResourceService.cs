using MassTransit;
using StubberNet.Core.Models.Dynamic;
using StubberNet.Core.Models.Resource;
using StubberNet.Core.Objects;

namespace StubberNet.Core.Services;

public interface IResourceService
{
	IAsyncEnumerable<Resource> GetResources(GetItemsQuery<Resource>? query, CancellationToken cancellationToken);

	Task<Resource> GetResource(NewId resourceId, CancellationToken cancellationToken);

	Task<Resource> AddResource(AddResourceModel addResourceModel, CancellationToken cancellationToken);

	IAsyncEnumerable<Token> GetResourceData(NewId resourceId, CancellationToken cancellationToken);
}