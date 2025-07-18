using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookstore.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure services here
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO: Implement or create proper LoggingSetup class
            // LoggingSetup.ConfigureLogging();

            // Configure basic logging
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation("Application starting up");

            // TODO: Implement or create proper ConfigurationSetup class
            // ConfigurationSetup.ConfigureConfiguration();

            // TODO: Implement or create proper DependencyInjectionSetup class
            // DependencyInjectionSetup.ConfigureDependencyInjection(app);

            // TODO: Implement or create proper AuthenticationConfig class
            // AuthenticationConfig.ConfigureAuthentication(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}