using MassTransit;
using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Models.Resource;

namespace StubberNet.Core.Repository;

internal interface IRepositoryFacade
{
	IRepository<Project, NewId> ProjectRepository { get; }

	IRepository<Resource, NewId> ResourceRepository { get; }

	IRepository<ApiEndpoint, NewId> ApiEndpointRepository { get; }
}