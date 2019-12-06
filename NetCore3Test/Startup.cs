using System;
using System.IO;
using Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore3Test.Extensions;
using NetCore3Test.Filters;
using NetCore3Test.Middleware;
using NLog;
using Persistence;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace NetCore3Test
{
    public sealed class Startup : IDisposable
    {
        private readonly IWebHostEnvironment _env;
        private readonly Container _container;

        public Startup(IWebHostEnvironment env)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            _env = env;
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = ConfigFileLoader.GetAppSettings(_env);
            services.AddLocalization();
            services.AddControllers();
            services.AddOptions();
            services.AddScoped<ValidateModelFilterAttribute>();

            services.ConfigureMVC();
            services.ConfigureDependencyInjection(_container);
            services.ConfigureDb(config);
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureSwagger();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSimpleInjector(_container, options => app.UseMiddleware<ExceptionMiddleware>(_container));

            app.UseHttpsRedirection();
            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseCors("CorsPolicy");
            app.UseCookiePolicy();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            //app.UseStaticFiles();
            app.UseSecurityHeaders();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            // Comment out to disable automatic migration/seeding
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetService<ApplicationContext>().Database.Migrate();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
