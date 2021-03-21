using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System;

namespace CleanArchitecture.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);


            var allowedOrigins = Configuration["Cors:AllowedOrigins"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                                  builder => builder.WithOrigins(origins: allowedOrigins)
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod());
            });


            services.AddFeatureManagement();

            services.AddControllers().AddFluentValidation();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Diplomat Onion template",
                    Description = "Swagger endpoint"
                });
                c.OperationFilter<CustomHeaderSwaggerAttribute>();

                //// Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.UseDeveloperExceptionPage();

            #region Swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diplomat Onion template");
            });

            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
