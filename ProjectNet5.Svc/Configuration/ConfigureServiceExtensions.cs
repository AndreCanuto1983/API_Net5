using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class ConfigureServiceExtensions
    {
        //Convert json which is CamelCase to lowercase when using System.Text.Json
        public static void Configurations(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
