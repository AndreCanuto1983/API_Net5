using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class ServiceExtensions
    {
        //Convert json which is CamelCase to lowercase when using System.Text.Json
        public static void ServiceSettings(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
