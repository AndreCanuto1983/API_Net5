using Core.Models.Base;
using Domain.Models;
using Infra.Configuration;
using Infra.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Configuration;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
                
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurando o acesso ao bd
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //cria e utiliza o context na memória, qdo a requisição terminar ele limpa o context da memória.
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();

            services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Interface Implements
            InterfaceConfig.InterfaceImplementation(ref services);                    

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Configurando JWT
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

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

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddCors();

            //para setar um cors específico
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:8080", "https://simis-ccef7.web.app", "http://simisapp.ddns.net:3000"));
            });

            services.AddControllers();
            services.AddSwaggerConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            //seed data initialize
            SeedData.Initialize(context, userManager);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseAuthorization(); //para autorizar roles

            app.UseMvc();

            //usar swagger
            app.UseSwaggerConfiguration();
        }
    }
}