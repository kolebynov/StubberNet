using MassTransit;
using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Objects;
using StubberNet.Core.Repository;

namespace StubberNet.Core.Services;

internal sealed class ApiEndpointService : IApiEndpointService
{
	private readonly IRepository<ApiEndpoint, NewId> _apiEndpointRepository;
	private readonly IProjectService _projectService;
	private readonly IResourceService _resourceService;

	public ApiEndpointService(IRepository<ApiEndpoint, NewId> apiEndpointRepository, IProjectService projectService,
		IResourceService resourceService)
	{
		_apiEndpointRepository = apiEndpointRepository ?? throw new ArgumentNullException(nameof(apiEndpointRepository));
		_projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
		_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
	}

	public IAsyncEnumerable<ApiEndpoint> GetApiEndpoints(GetItemsQuery<ApiEndpoint>? query, CancellationToken cancellationToken) =>
		_apiEndpointRepository.GetItems(query?.ToRepositoryQuery(), cancellationToken);

	public async Task<ApiEndpoint> AddApiEndpoint(AddApiEndpointModel addApiEndpointModel, CancellationToken cancellationToken)
	{
		var project = await _projectService.GetProject(addApiEndpointModel.ProjectId, cancellationToken);
		_ = await _resourceService.GetResource(addApiEndpointModel.ResponseDefinition.ReturnResourceId, cancellationToken);

		var newEndpoint = new ApiEndpoint(NewId.Next(), project.Id, addApiEndpointModel.RequestDefinition,
			addApiEndpointModel.ResponseDefinition);
		await _apiEndpointRepository.Add(newEndpoint, cancellationToken);

		return newEndpoint;
	}
}