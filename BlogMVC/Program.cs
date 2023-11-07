using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using BlogMVC.BLL;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlogMVCContext") ?? throw new InvalidOperationException("Connection string 'BlogMVCContext' not found.");
Startup.Configure(builder.Services, connectionString);

// Add services to the container.
//builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<BlogMVCContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Account/Login");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BlogPosts}/{action=Index}/{id?}");

app.Run();
