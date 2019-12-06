using System;
using System.Diagnostics;
using Domain.Models;
using LoggerService;
using LoggerService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NetCore3Test.Filters;
using Persistence;
using Service.Interfaces;
using Service.Services;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;

namespace NetCore3Test.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                //.AllowCredentials());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureMVC(this IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(ValidateModelFilterAttribute));

                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
            });
        }

        public static void ConfigureDb(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            }, ServiceLifetime.Transient);
        }

        public static void ConfigureDependencyInjection(this IServiceCollection services, Container container)
        {
            if (container == null)
                return;

            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    .AddControllerActivation()
                    .AddViewComponentActivation();
                options.AddLocalization();
            });

            // Register services
            container.Register<ILoggerManager, LoggerManager>(Lifestyle.Singleton);
            container.Register<ISimpleService, SimpleService>();
            container.Register<IUnitOfWork, UnitOfWork<ApplicationContext>>();
            container.Register<IRepository<SimpleEntity>, Repository<ApplicationContext, SimpleEntity>>();

            // Register controllers DI resolution
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));

            // Wrap AspNet requests into Simpleinjector's scoped lifestyle
            services.UseSimpleInjectorAspNetRequestScoping(container);

        }
        //TODO: Apply your security headers
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                //context.Response.Headers.Add(
                //    "Content-Security-Policy",
                //    "default-src 'self'; " +
                //    "img-src 'self'; " +
                //    "font-src 'self'; " +
                //    "style-src 'self'; " +
                //    "script-src 'self'; " +
                //    "frame-src 'self';" +
                //    "connect-src 'self';");
                await next().ConfigureAwait(false);
            });
        }
    }
}
