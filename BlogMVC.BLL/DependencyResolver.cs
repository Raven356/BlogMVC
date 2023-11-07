using Microsoft.Extensions.DependencyInjection;

namespace BlogMVC.BLL
{
    public class DependencyResolver
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            BlogMVC.DAL.DependencyResolver.Configure(services, connectionString);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyResolver).Assembly));

            services.AddAutoMapper(typeof(CoreMappingProfile).Assembly);
        }
    }
}
