using CoExittor.Front.Services;
using CoExittor.Front.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

AuthSingleCookieWithFront(builder.Services);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient("BackendClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "http://localhost:5087/");
});
builder.Services.AddScoped<IBackendClient, BackendClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


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
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
        });
}