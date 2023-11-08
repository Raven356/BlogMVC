using BlogMVC.BLL.Services.AccountsService;
using BlogMVC.BLL.Services.AuthorsService;
using BlogMVC.BLL.Services.BlogPostService;
using BlogMVC.BLL.Services.CategoriesService;
using Microsoft.Extensions.DependencyInjection;

namespace BlogMVC.BLL
{
    public class DependencyResolver
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            BlogMVC.DAL.DependencyResolver.Configure(services, connectionString);

            services.AddScoped<IAuthorsService, AuthorsService>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IAccountService, AccountService>();
            

            services.AddAutoMapper(typeof(CoreMappingProfile).Assembly);
        }
    }
}
