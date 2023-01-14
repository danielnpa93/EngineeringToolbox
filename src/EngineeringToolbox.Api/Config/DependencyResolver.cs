using EngineeringToolbox.Application.Interfaces;
using EngineeringToolbox.Application.Services;
using EngineeringToolbox.Domain.Extensions;
using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Domain.Repositories;
using EngineeringToolbox.Infrastructure.Repositories;
using System.Reflection;

namespace EngineeringToolbox.Api.Config
{
    public static class DependencyResolver
    {
        public static void AddDependenciesResolver(this IServiceCollection services)
        {
            AddServices(services);
            AddRepositories(services);
            AddUtils(services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAuthServices, AuthServices>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IIdentityRepository, IdentityRepository>();
        }
        private static void AddUtils(IServiceCollection services)
        {
            services.AddScoped<NotificationContext>();
            services.AddAutoMapper(GetAssemblies());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AppUser>();
        }

        private static Assembly[] GetAssemblies()
        {
            return new Assembly[]
            {
                Assembly.Load("EngineeringToolbox.Api"),
                Assembly.Load("EngineeringToolbox.Application"),
                Assembly.Load("EngineeringToolbox.Domain"),
                Assembly.Load("EngineeringToolbox.Infrastructure"),
            };
        }
    }
}
