using Microsoft.AspNetCore.Mvc;
using StubberNet.Core.Abstractions;
using StubberNet.Core.Models;
using StubberNet.Web.Dto;

namespace StubberNet.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
	private readonly ILogger<ProjectsController> _logger;
	private readonly IRepositoryFacade _repositoryFacade;

	public ProjectsController(ILogger<ProjectsController> logger, IRepositoryFacade repositoryFacade)
	{
		_logger = logger;
		_repositoryFacade = repositoryFacade;
	}

	[HttpGet]
	public async Task<IEnumerable<ProjectDto>> Get()
	{
		var projects = await _repositoryFacade.ProjectRepository.GetItems(null, CancellationToken.None);
		return projects.Select(x => new ProjectDto { Name = x.Name, Id = x.Id.Value });
	}

	[HttpPost]
	public Task Add([FromBody] ProjectDto projectDto)
	{
		return _repositoryFacade.ProjectRepository.Add(
			new Project(ProjectId.Create(projectDto.Id), projectDto.Name), CancellationToken.None);
	}
}