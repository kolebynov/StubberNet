using MassTransit;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Objects;
using StubberNet.Core.Repository;

namespace StubberNet.Core.Services;

internal sealed class ProjectService : IProjectService
{
	private readonly IRepository<Project, NewId> _projectRepository;

	public ProjectService(IRepository<Project, NewId> projectRepository)
	{
		_projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
	}

	public IAsyncEnumerable<Project> GetProjects(GetItemsQuery<Project>? query, CancellationToken cancellationToken) =>
		_projectRepository.GetItems(query?.ToRepositoryQuery(), cancellationToken);

	public Task<Project> GetProject(NewId projectId, CancellationToken cancellationToken) =>
		_projectRepository.GetById(projectId, cancellationToken);

	public async Task<Project> AddProject(AddProjectModel addProjectModel, CancellationToken cancellationToken)
	{
		var newProject = new Project(NewId.Next(), addProjectModel.Name, addProjectModel.BasePath);
		await _projectRepository.Add(newProject, cancellationToken);
		return newProject;
	}

	public Task RemoveProject(NewId projectId, CancellationToken cancellationToken) =>
		_projectRepository.Delete(projectId, cancellationToken);
}