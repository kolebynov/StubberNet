using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using StubberNet.Core.Abstractions;
using StubberNet.Core.Extensions;
using StubberNet.Core.Models;
using StubberNet.Infrastructure.LiteDb.Abstractions;
using StubberNet.Infrastructure.LiteDb.Internal;
using StubberNet.Infrastructure.LiteDb.Settings;

namespace StubberNet.Infrastructure.LiteDb.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLiteDbInfrastructure(this IServiceCollection services, Action<LiteDbSettings>? configureAction = null)
	{
		_ = services ?? throw new ArgumentNullException(nameof(services));

		services.AddCoreServices();

		services.TryAddSingleton<ILiteDatabase>(sp =>
		{
			SetupBsonMapper(BsonMapper.Global);

			var settings = sp.GetRequiredService<IOptions<LiteDbSettings>>().Value;
			return new LiteDatabase(settings.ConnectionString, BsonMapper.Global);
		});

		services.TryAddSingleton(ResolveLiteCollection<Project>);
		services.TryAddSingleton(typeof(IRepository<,>), typeof(Repository<,>));

		if (configureAction != null)
		{
			services.Configure(configureAction);
		}

		return services;
	}

	private static ILiteCollection<T> ResolveLiteCollection<T>(IServiceProvider serviceProvider)
	{
		var liteCollection = serviceProvider.GetRequiredService<ILiteDatabase>().GetCollection<T>();
		var configurator = serviceProvider.GetServices<ILiteCollectionConfigurator<T>>().FirstOrDefault();
		configurator?.Configure(liteCollection);

		return liteCollection;
	}

	private static void SetupBsonMapper(BsonMapper bsonMapper)
	{
		bsonMapper.RegisterType(x => new BsonValue(x.Value), x => ProjectId.Create(x.AsString));

		bsonMapper.Entity<Project>()
			.Id(x => x.Id, false)
			.Ctor(x => new Project(ProjectId.Create(x["_id"].AsString), x["name"].AsString));
	}
}