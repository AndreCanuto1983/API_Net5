using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class ConfigureServiceExtensions
    {
        //Convert json which is CamelCase to lowercase when using System.Text.Json
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);
        }
    }
}
