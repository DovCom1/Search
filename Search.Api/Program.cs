using Search.API.Middleware;
using Search.Contract.Manager;
using Search.Contract.Service;
using Search.Service.Manager;
using Search.Service.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddHttpClient<IUserSearchService, UserSearchService>(client =>
{
    client.BaseAddress = new Uri("http://user-service:8080");
});

builder.Services.AddScoped<IUserSearchManager, UserSearchManager>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();


