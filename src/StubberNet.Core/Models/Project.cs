using StubberNet.Core.Abstractions;

namespace StubberNet.Core.Models;

public sealed class Project : IIdentifiable<ProjectId>
{
	public ProjectId Id { get; }

	public string Name { get; }

	public Project(ProjectId id, string name)
	{
		if (id.Value == null)
		{
			throw new ArgumentException("Value cannot be null.", nameof(id));
		}

		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
		}

		Id = id;
		Name = name;
	}
}