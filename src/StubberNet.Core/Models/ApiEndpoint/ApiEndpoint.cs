using MassTransit;

namespace StubberNet.Core.Models.ApiEndpoint;

public sealed class ApiEndpoint : IIdentifiable<NewId>
{
	public NewId Id { get; }

	public NewId ProjectId { get; }

	public RequestDefinition RequestDefinition { get; }

	public ResponseDefinition ResponseDefinition { get; }

	public ApiEndpoint(NewId id, NewId projectId, RequestDefinition requestDefinition, ResponseDefinition responseDefinition)
	{
		Id = id;
		RequestDefinition = requestDefinition ?? throw new ArgumentNullException(nameof(requestDefinition));
		ResponseDefinition = responseDefinition ?? throw new ArgumentNullException(nameof(responseDefinition));
		ProjectId = projectId;
	}
}