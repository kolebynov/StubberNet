using System.ComponentModel.DataAnnotations;

namespace StubberNet.Web.Dto;

public sealed class ProjectDto
{
	[Required]
	public Guid Id { get; init; }

	[Required]
	public string Name { get; init; } = null!;

	[Required]
	[RegularExpression("^[\\w_-\\d]+$")]
	public string BasePath { get; init; } = null!;
}