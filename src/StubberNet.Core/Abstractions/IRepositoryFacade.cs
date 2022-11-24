using StubberNet.Core.Models;

namespace StubberNet.Core.Abstractions;

public interface IRepositoryFacade
{
	public IRepository<Project, ProjectId> ProjectRepository { get; }
}