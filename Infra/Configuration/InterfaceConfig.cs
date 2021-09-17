using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Infra.Repository;
using Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Configuration
{
    public static class InterfaceConfig
    {
        public static void InterfaceImplementation(ref IServiceCollection services)
        {
            //Register General
            services.AddScoped<IGenerateToken, TokenClaimService>();                 
            services.AddScoped<IEmailService, SendGridEmailService>();

            //Register Repository
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
