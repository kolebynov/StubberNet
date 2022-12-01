using Microsoft.AspNetCore.Mvc;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Services;
using StubberNet.Web.Dto;

namespace StubberNet.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
	private readonly ILogger<ProjectsController> _logger;
	private readonly IProjectService _projectService;

	public ProjectsController(ILogger<ProjectsController> logger, IProjectService projectService)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
	}

	[HttpGet]
	public IAsyncEnumerable<ProjectDto> Get()
	{
		return _projectService.GetProjects(null, CancellationToken.None)
			.Select(x => new ProjectDto { Name = x.Name, Id = x.Id.ToGuid(), BasePath = x.BasePath });
	}

	[HttpPost]
	public Task Add([FromBody] ProjectDto projectDto)
	{
		return _projectService.AddProject(
			new AddProjectModel { Name = projectDto.Name, BasePath = projectDto.BasePath }, CancellationToken.None);
	}
}