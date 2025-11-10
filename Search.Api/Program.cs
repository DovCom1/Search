using Microsoft.OpenApi.Models;
using Search.API.Middleware;
using Search.Contract.Manager;
using Search.Contract.Service;
using Search.Service.Manager;
using Search.Service.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Search API", 
        Version = "v1",
        Description = "API для работы с Search"
    });
});

builder.Services.AddHttpClient<IUserSearchService, UserSearchService>(client =>
{
    client.BaseAddress = new Uri("http://user-service:8080");
});

builder.Services.AddScoped<IUserSearchManager, UserSearchManager>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search v1");
        c.RoutePrefix = "swagger";
    });
}
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();


