using BlogMVC.BLL.AuthorsOperations.AuthorsService;
using BlogMVC.BLL.BlogPostOperations.BlogPostService;
using BlogMVC.BLL.CategoriesOperations.CategoriesService;
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

            services.AddAutoMapper(typeof(CoreMappingProfile).Assembly);
        }
    }
}
