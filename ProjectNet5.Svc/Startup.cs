using Api.Configuration;
using Domain.Models;
using Infra.Configuration;
using Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectNet5.Svc.Configuration;
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
            DBSettings.Configurations(services, Configuration);            

            InterfaceSettings.Configurations(ref services);

            ConfigureServiceExtensions.Configurations(services);

            JwtSettings.Configurations(services, Configuration);

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddCors();
            //set especific cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:8080"));
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

            //for authorize roles
            app.UseAuthorization(); 

            app.UseMvc();

            //using swagger
            app.UseSwaggerConfiguration();
        }
    }
}