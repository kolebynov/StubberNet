using MassTransit;

namespace StubberNet.Core.Models.Project;

public sealed class Project : IIdentifiable<NewId>
{
	public NewId Id { get; }

	public string Name { get; }

	public string BasePath { get; }

	public Project(NewId id, string name, string basePath)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
		}

		if (string.IsNullOrWhiteSpace(basePath))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(basePath));
		}

		Id = id;
		Name = name;
		BasePath = basePath;
	}
}