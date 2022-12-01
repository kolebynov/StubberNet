using MassTransit;

namespace StubberNet.Core.Models.ApiEndpoint;

public sealed class AddApiEndpointModel
{
	public NewId ProjectId { get; init; }

	public RequestDefinition RequestDefinition { get; init; } = null!;

	public ResponseDefinition ResponseDefinition { get; init; } = null!;
}