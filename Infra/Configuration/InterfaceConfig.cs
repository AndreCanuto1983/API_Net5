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
            services.AddTransient<IGenerateToken, TokenClaimService>();                 
            services.AddTransient<IEmailService, SendGridEmailService>();

            //Register Repository
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
