namespace StubberNet.Core.Models.Project;

public sealed class AddProjectModel
{
	public string Name { get; init; } = null!;

	public string BasePath { get; init; } = null!;
}