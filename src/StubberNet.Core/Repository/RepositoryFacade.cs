using MassTransit;
using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Models.Resource;

namespace StubberNet.Core.Repository;

internal sealed class RepositoryFacade : IRepositoryFacade
{
	public IRepository<Project, NewId> ProjectRepository { get; }

	public IRepository<Resource, NewId> ResourceRepository { get; }

	public IRepository<ApiEndpoint, NewId> ApiEndpointRepository { get; }

	public RepositoryFacade(IRepository<Project, NewId> projectRepository, IRepository<Resource, NewId> resourceRepository,
		IRepository<ApiEndpoint, NewId> apiEndpointRepository)
	{
		ProjectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
		ResourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
		ApiEndpointRepository = apiEndpointRepository ?? throw new ArgumentNullException(nameof(apiEndpointRepository));
	}
}