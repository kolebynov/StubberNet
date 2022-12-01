using MassTransit;
using StubberNet.Core.Models.Dynamic;
using StubberNet.Core.Models.Resource;
using StubberNet.Core.Objects;
using StubberNet.Core.Repository;

namespace StubberNet.Core.Services;

internal sealed class ResourceService : IResourceService
{
	private readonly IRepository<Resource, NewId> _resourceRepository;
	private readonly IProjectService _projectService;
	private readonly IResourceDataRepository _resourceDataRepository;

	public ResourceService(IRepository<Resource, NewId> resourceRepository, IProjectService projectService,
		IResourceDataRepository resourceDataRepository)
	{
		_resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
		_projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
		_resourceDataRepository = resourceDataRepository ?? throw new ArgumentNullException(nameof(resourceDataRepository));
	}

	public IAsyncEnumerable<Resource> GetResources(GetItemsQuery<Resource>? query, CancellationToken cancellationToken) =>
		_resourceRepository.GetItems(query?.ToRepositoryQuery(), cancellationToken);

	public Task<Resource> GetResource(NewId resourceId, CancellationToken cancellationToken) =>
		_resourceRepository.GetById(resourceId, cancellationToken);

	public async Task<Resource> AddResource(AddResourceModel addResourceModel, CancellationToken cancellationToken)
	{
		await _projectService.GetProject(addResourceModel.ProjectId, cancellationToken);
		var newResource = new Resource(NewId.Next(), addResourceModel.Name, addResourceModel.ProjectId,
			addResourceModel.ResourceData.Definition);
		await _resourceRepository.Add(newResource, cancellationToken);
		await _resourceDataRepository.SetResourceData(newResource.Id, addResourceModel.ResourceData.Data, cancellationToken);

		return newResource;
	}

	public IAsyncEnumerable<Token> GetResourceData(NewId resourceId, CancellationToken cancellationToken) =>
		_resourceDataRepository.GetResourceData(resourceId, cancellationToken);
}