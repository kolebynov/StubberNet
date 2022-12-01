using System.Globalization;
using LiteDB;
using LiteDB.Engine;
using MassTransit;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using StubberNet.Core.Extensions;
using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Models.Dynamic;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Models.Resource;
using StubberNet.Core.Repository;
using StubberNet.Repository.LiteDb.Settings;

namespace StubberNet.Repository.LiteDb.Extensions;

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
			var db = new LiteDatabase(settings.ConnectionString, BsonMapper.Global);
			if (db.Collation.SortOptions != CompareOptions.None)
			{
				db.Rebuild(new RebuildOptions { Collation = new Collation(CultureInfo.CurrentCulture.LCID, CompareOptions.None) });
			}

			return new LiteDatabaseWrapper(db, sp);
		});

		services.TryAddSingleton(ResolveLiteCollection<Project>);
		services.TryAddSingleton(ResolveLiteCollection<Resource>);
		services.TryAddSingleton(ResolveLiteCollection<ApiEndpoint>);
		services.TryAddSingleton(typeof(IRepository<,>), typeof(Repository<,>));
		services.TryAddSingleton<IResourceDataRepository, ResourceDataRepository>();
		services.TryAddSingleton(BsonMapper.Global);

		if (configureAction != null)
		{
			services.Configure(configureAction);
		}

		return services;
	}

	private static ILiteCollection<T> ResolveLiteCollection<T>(IServiceProvider serviceProvider) =>
		serviceProvider.GetRequiredService<ILiteDatabase>().GetCollection<T>();

	private static void SetupBsonMapper(BsonMapper bsonMapper)
	{
		bsonMapper.Entity<Project>()
			.Id(x => x.Id, false)
			.Ctor(x => new Project(x["_id"].AsGuid.ToNewId(), x["name"].AsString, x["basePath"].AsString));

		bsonMapper.Entity<Resource>()
			.Id(x => x.Id, false)
			.Ctor(x => new Resource(x["_id"].AsGuid.ToNewId(), x["name"].AsString, x["projectId"].AsGuid.ToNewId(),
				bsonMapper.Deserialize<IResourceDefinition>(x["resourceDefinition"])));

		bsonMapper.Entity<ApiEndpoint>()
			.Id(x => x.Id, false)
			.Ctor(x => new ApiEndpoint(x["_id"].AsGuid.ToNewId(), x["projectId"].AsGuid.ToNewId(),
				bsonMapper.Deserialize<RequestDefinition>(x["requestDefinition"]),
				bsonMapper.Deserialize<ResponseDefinition>(x["responseDefinition"])));

		bsonMapper.Entity<RequestDefinition>()
			.Ctor(x => new RequestDefinition(
				bsonMapper.Deserialize<HttpMethod>(x["method"]), bsonMapper.Deserialize<RoutePattern>(x["routePattern"])));

		bsonMapper.RegisterType(ToBson, FromBson);
		bsonMapper.RegisterType(ToBson, x => (DynamicObject)FromBson(x));
		bsonMapper.RegisterType(ToBson, x => (DynamicArray)FromBson(x));
		bsonMapper.RegisterType(ToBson, x => (StringValue)FromBson(x));
		bsonMapper.RegisterType(x => x.Method, x => new HttpMethod(x.AsString));
		bsonMapper.RegisterType(x => new BsonValue(x.ToGuid()), x => x.AsGuid.ToNewId());
		bsonMapper.RegisterType(x => x.RawText, x => RoutePatternFactory.Parse(x.AsString));
	}

	private static BsonValue ToBson(Token token) =>
		token switch
		{
			DynamicObject dynamicObject => ToBson(dynamicObject),
			DynamicArray dynamicArray => ToBson(dynamicArray),
			StringValue stringValue => new BsonValue(stringValue.Value),
			_ => throw new NotSupportedException($"Can't convert {token.GetType()} to BSON"),
		};

	private static BsonDocument ToBson(DynamicObject dynamicObject) => new(dynamicObject.ToDictionary(x => x.Key, x => ToBson(x.Value)));

	private static BsonArray ToBson(DynamicArray dynamicArray) => new(dynamicArray.Select(ToBson));

	private static Token FromBson(BsonValue bsonValue) =>
		bsonValue switch
		{
			BsonDocument bsonDocument => FromBson(bsonDocument),
			BsonArray bsonArray => FromBson(bsonArray),
			{ IsString: true } => new StringValue(bsonValue.AsString),
			_ => throw new NotSupportedException($"Can't convert BSON value {bsonValue.Type} to dynamic value"),
		};

	private static DynamicObject FromBson(BsonDocument bsonDocument) => new(bsonDocument.Where(x => !x.Value.IsObjectId).ToDictionary(x => x.Key, x => FromBson(x.Value)));

	private static DynamicArray FromBson(BsonArray bsonArray) => new(bsonArray.Where(x => !x.IsObjectId).Select(FromBson).ToArray());
}