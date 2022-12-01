using Microsoft.AspNetCore.Routing.Patterns;
using StubberNet.Core.Models.ApiEndpoint;
using StubberNet.Core.Models.Dynamic;
using StubberNet.Core.Models.Project;
using StubberNet.Core.Models.Resource;
using StubberNet.Core.Services;
using StubberNet.Repository.LiteDb.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLiteDbInfrastructure();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
	var resourceService = scope.ServiceProvider.GetRequiredService<IResourceService>();
	var apiEndpointService = scope.ServiceProvider.GetRequiredService<IApiEndpointService>();

	if (!(await projectService.GetProjects(null, CancellationToken.None).AnyAsync()))
	{
		var project = await projectService.AddProject(new AddProjectModel { Name = "project", BasePath = "base" }, CancellationToken.None);
		var resource = await resourceService.AddResource(
			new AddResourceModel
			{
				Name = "resource",
				ProjectId = project.Id,
				ResourceData = new StaticResourceData(new DynamicArray(new Token[] { "value1", "value2", "value3" })),
			},
			CancellationToken.None);
		var apiEndpoint = await apiEndpointService.AddApiEndpoint(
			new AddApiEndpointModel
			{
				ProjectId = project.Id,
				RequestDefinition = new RequestDefinition(HttpMethod.Get, RoutePatternFactory.Parse("get")),
				ResponseDefinition = new ResponseDefinition(resource.Id),
			},
			CancellationToken.None);
		var data = await resourceService.GetResourceData(resource.Id, CancellationToken.None).ToArrayAsync();
	}

	var invoker = scope.ServiceProvider.GetRequiredService<IApiEndpointInvoker>();
	var res = await invoker.InvokeApiEndpoint(new Request(HttpMethod.Get, "/Base/Get"), CancellationToken.None);
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Map("/projects/{**route}", async (string route, IApiEndpointInvoker apiInvoker, HttpContext httpContext) =>
{
	var response = await apiInvoker.InvokeApiEndpoint(new Request(new HttpMethod(httpContext.Request.Method), route), httpContext.RequestAborted);
	httpContext.Response.StatusCode = (int)response.StatusCode;
	foreach (var (name, value) in response.Headers)
	{
		httpContext.Response.Headers[name] = value;
	}

	await response.WriteResponseBodyAsync(httpContext.Response.Body, httpContext.RequestAborted);
});

app.Run();