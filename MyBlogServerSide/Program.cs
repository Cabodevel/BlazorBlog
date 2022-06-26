using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Data.Interfaces;
using MyBlog.Data.Models;
using MyBlogServerSide.Authentication;
using MyBlogServerSide.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyBlogDB");

builder.Services.AddDbContextFactory<MyBlogDbContext>(opt => opt.UseSqlite(connectionString));
builder.Services.AddDbContext<MyBlogDbContext>(opt => opt.UseSqlite(connectionString));
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<MyBlogDbContext>();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();

builder.Services.AddScoped<IMyBlogApi, MyBlogApiServerSide>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<MyBlogDbContext>();
        db.Database.Migrate();
    }
    catch (Exception e)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        logger.LogError(e, "Db migration error");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
