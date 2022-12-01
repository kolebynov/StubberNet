using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using StubberNet.Core.Exceptions;
using StubberNet.Core.Infrastructure;
using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Objects;
using StubberNet.Core.Repository;

namespace StubberNet.Core.Services;

internal sealed class ApiEndpointInvoker : IApiEndpointInvoker
{
	private static readonly RouteValueDictionary EmptyRouteValueDictionary = new();

	private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
	{
		Converters = { new TokenJsonConverter() },
	};

	private readonly IProjectService _projectService;
	private readonly IApiEndpointService _apiEndpointService;
	private readonly IResourceDataRepository _resourceDataRepository;

	public ApiEndpointInvoker(IProjectService projectService, IApiEndpointService apiEndpointService,
		IResourceDataRepository resourceDataRepository)
	{
		_projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
		_apiEndpointService = apiEndpointService ?? throw new ArgumentNullException(nameof(apiEndpointService));
		_resourceDataRepository = resourceDataRepository ?? throw new ArgumentNullException(nameof(resourceDataRepository));
	}

	public async Task<Response> InvokeApiEndpoint(Request request, CancellationToken cancellationToken)
	{
		var segments = request.Uri.Segments.Select(x => x.Trim('/')).Where(x => !string.IsNullOrEmpty(x)).ToArray();
		if (segments.Length < 2)
		{
			throw new ValidationException("Request uri path must contain at least 2 segments");
		}

		var projectBasePath = segments[0];
		var project = await _projectService
			.GetProjects(new GetItemsQuery<Project> { Filters = { p => p.BasePath.ToUpper() == projectBasePath.ToUpper() } }, cancellationToken)
			.SingleOrDefaultAsync(cancellationToken);

		if (project == null)
		{
			throw new ItemNotFoundException($"Project with base path {projectBasePath} was not found");
		}

		var pathForTemplate = new PathString($"/{string.Join('/', segments[1..])}");
		var (endpoint, routeValues) = await _apiEndpointService
			.GetApiEndpoints(new GetItemsQuery<ApiEndpoint> { Filters = { x => x.ProjectId == project.Id } }, cancellationToken)
			.Where(x => x.RequestDefinition.Method == request.Method)
			.Select(x =>
			{
				var templateMatcher = new TemplateMatcher(new RouteTemplate(x.RequestDefinition.RoutePattern), EmptyRouteValueDictionary);
				var values = new RouteValueDictionary();
				return (ApiEndpoint: x, SuccessMatch: templateMatcher.TryMatch(pathForTemplate, values), RouteValues: values);
			})
			.Where(x => x.SuccessMatch)
			.Select(x => (x.ApiEndpoint, x.RouteValues))
			.FirstOrDefaultAsync(cancellationToken);

		if (endpoint == null)
		{
			throw new ItemNotFoundException($"API endpoint for path {pathForTemplate} was not found");
		}

		var resourceData = await _resourceDataRepository.GetResourceData(endpoint.ResponseDefinition.ReturnResourceId, cancellationToken)
			.ToArrayAsync(cancellationToken);

		return new Response(
			HttpStatusCode.OK,
			new Dictionary<string, string>
			{
				{ "content-type", "application/json" },
			},
			(r, ct) => JsonSerializer.SerializeAsync(r, resourceData, SerializerOptions, ct));
	}
}