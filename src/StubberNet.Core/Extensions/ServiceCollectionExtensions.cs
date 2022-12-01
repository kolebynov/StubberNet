using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StubberNet.Core.Repository;
using StubberNet.Core.Services;

namespace StubberNet.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCoreServices(this IServiceCollection services)
	{
		_ = services ?? throw new ArgumentNullException(nameof(services));
		services.TryAddScoped<IRepositoryFacade, RepositoryFacade>();
		services.TryAddScoped<IProjectService, ProjectService>();
		services.TryAddScoped<IResourceService, ResourceService>();
		services.TryAddScoped<IApiEndpointService, ApiEndpointService>();
		services.TryAddScoped<IApiEndpointInvoker, ApiEndpointInvoker>();

		return services;
	}
}