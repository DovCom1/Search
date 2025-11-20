using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Search.API.Middleware;
using Search.Contract.Manager;
using Search.Contract.Service;
using Search.Service.Manager;
using Search.Service.Request;
using Search.Service.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddControllers();

builder.Services.Configure<RequestDomains>(builder.Configuration.GetSection("RequestDomains"));

builder.Services.AddSingleton<RequestFactory>();
builder.Services.AddHttpClient();

// builder.Services.AddHttpClient<IUserSearchService, UserSearchService>((sp, client) =>
// {
//     var domains = sp.GetRequiredService<IOptions<RequestDomains>>().Value;
//     client.BaseAddress = new Uri(domains.UserService);
// });

builder.Services.AddScoped<IUserSearchManager, UserSearchManager>();
builder.Services.AddScoped<IUserSearchService, UserSearchService>();

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


