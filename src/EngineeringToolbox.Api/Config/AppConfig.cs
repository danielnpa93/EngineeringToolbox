using EngineeringToolbox.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringToolbox.Api.Config
{
    public static class AppConfig
    {
        public static void AddAppConfig(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<DefaultExceptionFilterAttribute>();
                options.Filters.Add<ValidateModelFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            services.AddEndpointsApiExplorer();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void UseAppConfig(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityConfig();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });
        }
    }
}
