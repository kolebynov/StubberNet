using MassTransit;

namespace StubberNet.Core.Models.Resource;

public sealed class AddResourceModel
{
	public string Name { get; init; } = null!;

	public NewId ProjectId { get; init; }

	public IResourceData ResourceData { get; init; } = null!;
}