using ApiVersioning.Foundation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));    //Include <summary></summary> comments
    options.SwaggerDoc(Constants.SwaggerDocumentVersion, new OpenApiInfo
    {
        Version = Constants.SwaggerDocumentVersion,
        Title = "ApiVersioning",
        Description = "Sample usage of API versioning with Swagger"
    });
});
builder.Services.ConfigureSwaggerGen(options =>
{
    options.ResolveConflictingActions(ApiDescriptionConflictResolver.PreferDefaultOrLatestApiVersion(Constants.DefaultApiVersion)); //Custom conflicts resolver
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(Constants.DefaultApiVersion, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("X-Api-Version"),
        new MediaTypeApiVersionReader("version"),
        new QueryStringApiVersionReader("version"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Constants.Policies.AllowOrigin.Any, policy => policy.AllowAnyOrigin());
    options.AddPolicy(name: Constants.Policies.AllowOrigin.Google, policy => policy.WithOrigins("https://www.google.com"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/{Constants.SwaggerDocumentVersion}/swagger.json", $"Swagger {Constants.SwaggerDocumentVersion}"));
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();