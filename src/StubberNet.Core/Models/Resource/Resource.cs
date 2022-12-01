using MassTransit;

namespace StubberNet.Core.Models.Resource;

public sealed class Resource : IIdentifiable<NewId>
{
	public NewId Id { get; }

	public string Name { get; }

	public NewId ProjectId { get; }

	public IResourceDefinition ResourceDefinition { get; }

	public Resource(NewId id, string name, NewId projectId, IResourceDefinition resourceDefinition)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
		}

		Id = id;
		Name = name;
		ProjectId = projectId;
		ResourceDefinition = resourceDefinition ?? throw new ArgumentNullException(nameof(resourceDefinition));
	}
}