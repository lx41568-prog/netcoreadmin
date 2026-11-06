using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using NetCoreAdmin.Data;
using NetCoreAdmin.Models;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/", "Authenticated");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
});
builder.Services.AddAuthorization(options =>
    options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser()));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Denied";
        options.Cookie.Name = "NetCoreAdmin.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddSingleton<IPasswordHasher<SysUser>, PasswordHasher<SysUser>>();
builder.Services.AddSingleton<ISqlSugarClient>(_ => new SqlSugarScope(new ConnectionConfig
{
    ConnectionString = builder.Configuration.GetConnectionString("MySql")
        ?? throw new InvalidOperationException("ConnectionStrings:MySql is not configured."),
    DbType = DbType.MySql,
    IsAutoCloseConnection = true,
    InitKeyType = InitKeyType.Attribute
}));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

await DbInitializer.InitializeAsync(app.Services, app.Configuration);
await app.RunAsync();
