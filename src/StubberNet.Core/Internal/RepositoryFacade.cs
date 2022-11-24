using StubberNet.Core.Abstractions;
using StubberNet.Core.Models;

namespace StubberNet.Core.Internal;

internal sealed class RepositoryFacade : IRepositoryFacade
{
	public IRepository<Project, ProjectId> ProjectRepository { get; }

	public RepositoryFacade(IRepository<Project, ProjectId> projectRepository)
	{
		ProjectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
	}
}