using System.Text.Json;
using System.Text.Json.Serialization;
using CoExittor.Api.Application;
using CoExittor.Api.Infrastructure;
using CoExittor.Api.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException("Строка подключения SqlServer не найдена, критическая ошибка.");

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
AuthSingleCookieWithFront(builder.Services);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.UseMiddleware<CustomExceptionMiddleware>();
app.UseExceptionMiddleware();

app.MapControllers();

// Автомиграция
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    db.Database.Migrate();
}

app.Run();

// Cookie, которые издаются в backend и frontend, применимы везде
static void AuthSingleCookieWithFront(IServiceCollection services)
{
    services.AddDataProtection()
        .SetApplicationName("CoExittor") // одинаково в бекенде и хосте
        .PersistKeysToFileSystem(new DirectoryInfo(@"C:\shared-dp-keys"));

    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "CoExittor.AuthCookie";
            options.Cookie.Path = "/";
        });
}