using MassTransit;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Objects;

namespace StubberNet.Core.Services;

public interface IProjectService
{
	IAsyncEnumerable<Project> GetProjects(GetItemsQuery<Project>? query, CancellationToken cancellationToken);

	Task<Project> GetProject(NewId projectId, CancellationToken cancellationToken);

	Task<Project> AddProject(AddProjectModel addProjectModel, CancellationToken cancellationToken);

	Task RemoveProject(NewId projectId, CancellationToken cancellationToken);
}