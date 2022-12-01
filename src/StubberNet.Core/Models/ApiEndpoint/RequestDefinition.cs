using Microsoft.AspNetCore.Routing.Patterns;

namespace StubberNet.Core.Models.ApiEndpoint;

public sealed class RequestDefinition
{
	public HttpMethod Method { get; }

	public RoutePattern RoutePattern { get; }

	public RequestDefinition(HttpMethod method, RoutePattern routePattern)
	{
		Method = method ?? throw new ArgumentNullException(nameof(method));
		RoutePattern = routePattern ?? throw new ArgumentNullException(nameof(routePattern));
	}
}