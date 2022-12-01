using StubberNet.Core.Models;
using StubberNet.Core.Objects;

namespace StubberNet.Core.Repository;

internal interface IRepository<TModel, TId>
	where TModel : class, IIdentifiable<TId>
	where TId : IEquatable<TId>
{
	IAsyncEnumerable<TModel> GetItems(GetRepositoryItemsQuery<TModel>? getItemsQuery, CancellationToken cancellationToken);

	Task<TModel> GetById(TId id, CancellationToken cancellationToken);

	Task Add(TModel item, CancellationToken cancellationToken);

	Task Update(TModel item, CancellationToken cancellationToken);

	Task Delete(TId id, CancellationToken cancellationToken);
}