using Core.Models.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProjectNet5.Svc.Configuration
{
    public static class JwtSettings
    {
        public static void Configurations(IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; //requerer https
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //habilitar validação da chave
                    IssuerSigningKey = new SymmetricSecurityKey(key), //chave da linha 51
                    ValidateIssuer = true, //habilita a validaçao do emissor
                    ValidIssuer = appSettings.Emissor, //valida o emissor
                    ValidateAudience = true, //habilita a validação da origem do token
                    ValidAudience = appSettings.OriginValidate //valida a origem do token (url). Posso passar uma collection se eu quiser
                };
            });
        }
    }
}
