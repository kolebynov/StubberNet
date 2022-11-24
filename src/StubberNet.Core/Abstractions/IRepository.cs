using StubberNet.Core.Objects;

namespace StubberNet.Core.Abstractions;

public interface IRepository<TModel, TId>
	where TModel : class, IIdentifiable<TId>
	where TId : IEquatable<TId>
{
	Task<IReadOnlyCollection<TModel>> GetItems(GetRepositoryItemsQuery<TModel>? getItemsQuery, CancellationToken cancellationToken);

	Task<TModel> GetById(TId id);

	Task Add(TModel item, CancellationToken cancellationToken);

	Task Update(TModel item, CancellationToken cancellationToken);

	Task Delete(TModel item, CancellationToken cancellationToken);
}