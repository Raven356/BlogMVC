using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogMVC.BLL
{
    public class Startup
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IBlogMVCContext, BlogMVCContext>();

            services.AddDbContext<BlogMVCContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BlogMVCContext>();

            services.AddScoped<IRepository<Author>, Repository<Author>>();
            services.AddScoped<IRepository<BlogPost>, Repository<BlogPost>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Comment>, Repository<Comment>>();
            services.AddScoped<IRepository<User>, Repository<User>>();

            services.AddAutoMapper(typeof(CoreMappingProfile).Assembly);
        }
    }
}
