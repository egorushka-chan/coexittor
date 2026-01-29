using CoExittor.Api.Application;
using CoExittor.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Test
builder.Services.AddDataProtection()
    .SetApplicationName("CoExittor") // одинаково в обоих
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\shared-dp-keys"));

string connectionString = builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException("Строка подключения SqlServer не найдена, критическая ошибка.");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "CoExittor.AuthCookie";
        options.Cookie.Path = "/";
    });

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

app.MapControllers();

// Автомиграция
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    db.Database.Migrate();
}

app.Run();
