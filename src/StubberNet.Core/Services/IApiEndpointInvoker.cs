using StubberNet.Core.Models.ApiEndpoint;

namespace StubberNet.Core.Services;

public interface IApiEndpointInvoker
{
	Task<Response> InvokeApiEndpoint(Request request, CancellationToken cancellationToken);
}