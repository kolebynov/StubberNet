using MassTransit;

namespace StubberNet.Core.Models.ApiEndpoint;

public sealed class ResponseDefinition
{
	public NewId ReturnResourceId { get; }

	public ResponseDefinition(NewId returnResourceId)
	{
		ReturnResourceId = returnResourceId;
	}
}