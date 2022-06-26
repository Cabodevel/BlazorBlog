using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Data.Interfaces;
using MyBlog.Data.Models;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<MyBlogDbContext>(opt => opt.UseSqlite($"Data Source=../../MyBlog.db"));

builder.Services.AddDbContext<MyBlogDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("MyBlogDB")));

builder.Services.AddDefaultIdentity<AppUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyBlogDbContext>();


builder.Services.AddIdentityServer().AddApiAuthorization<AppUser, MyBlogDbContext>(options =>
{
    options.IdentityResources["openid"].UserClaims.Add("name");
    options.ApiResources.Single().UserClaims.Add("name");
    options.IdentityResources["openid"].UserClaims.Add("role");
    options.ApiResources.Single().UserClaims.Add("role");
});

JwtSecurityTokenHandler.DefaultInboundClaimFilter.Remove("role");

builder.Services.AddAuthentication().AddIdentityServerJwt();

builder.Services.AddScoped<IMyBlogApi, MyBlogApiServerSide>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
