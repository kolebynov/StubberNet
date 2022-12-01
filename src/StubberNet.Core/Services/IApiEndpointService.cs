using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Objects;

namespace StubberNet.Core.Services;

public interface IApiEndpointService
{
	IAsyncEnumerable<ApiEndpoint> GetApiEndpoints(GetItemsQuery<ApiEndpoint>? query, CancellationToken cancellationToken);

	Task<ApiEndpoint> AddApiEndpoint(AddApiEndpointModel addApiEndpointModel, CancellationToken cancellationToken);
}