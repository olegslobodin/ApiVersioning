using ApiVersioning.Foundation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    //Include <summary></summary> comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();