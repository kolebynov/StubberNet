using System.ComponentModel.DataAnnotations;

namespace StubberNet.Web.Dto;

public sealed class ProjectDto
{
	[Required]
	public string Id { get; init; } = null!;

	[Required]
	public string Name { get; init; } = null!;
}