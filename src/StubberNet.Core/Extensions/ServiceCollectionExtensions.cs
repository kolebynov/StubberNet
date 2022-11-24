using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StubberNet.Core.Abstractions;
using StubberNet.Core.Internal;

namespace StubberNet.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCoreServices(this IServiceCollection services)
	{
		_ = services ?? throw new ArgumentNullException(nameof(services));
		services.TryAddScoped<IRepositoryFacade, RepositoryFacade>();

		return services;
	}
}