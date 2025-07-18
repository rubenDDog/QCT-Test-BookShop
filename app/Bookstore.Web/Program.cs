
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using WebOptimizer;

    namespace Bookstore
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);
                
                // Store configuration in static ConfigurationManager
                ConfigurationManager.Configuration = builder.Configuration;
                
                // Add services to the container (formerly ConfigureServices)
                builder.Services.AddControllersWithViews();

                // Register areas
                builder.Services.AddMvc()
                    .AddMvcOptions(options => {
                        // Register global filters here if needed
                    });

                // Bundle configuration will be handled by WebOptimizer in .NET Core
                builder.Services.AddWebOptimizer(pipeline =>
                {
                    // Configure bundles similar to BundleConfig.RegisterBundles
                    // Example: pipeline.AddCssBundle("/css/bundle.css", "wwwroot/css/**/*.css");
                    // Example: pipeline.AddJsBundle("/js/bundle.js", "wwwroot/js/**/*.js");
                });

                // Add logging
                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
                builder.Logging.AddDebug();

                
                var app = builder.Build();
                
                // Configure the HTTP request pipeline (formerly Configure method)
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                
                // Use WebOptimizer middleware (replaces BundleConfig)
                app.UseWebOptimizer();

                app.UseRouting();

                app.UseAuthorization();

                // Configure exception handling
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        if (exception != null)
                        {
                            logger.LogError(exception, "Unhandled exception");
                        }

                        await Task.CompletedTask;
                    });
                });
                
                // Register routes (similar to RouteConfig.RegisterRoutes)
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Register area routes if needed
                app.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                app.Run();
            }

            private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
            {
                // Configure services based on appSettings from Web.config
                var environment = configuration["Environment"] ?? "Development";
                var authService = configuration["Services/Authentication"] ?? "local";
                var dbService = configuration["Services/Database"] ?? "local";
                var fileService = configuration["Services/FileService"] ?? "local";
                var imageValidationService = configuration["Services/ImageValidationService"] ?? "local";
                var loggingService = configuration["Services/LoggingService"] ?? "local";

                // Configure authentication services
                if (authService == "aws")
                {
                    // Configure AWS Cognito authentication when needed
                    // Example: services.AddAuthentication()...
                }

                // Configure file services
                if (fileService == "aws")
                {
                    // Configure AWS S3 file services when needed
                    // Example: services.AddSingleton<IFileService, AwsFileService>();
                }
                else
                {
                    // Configure local file services
                    // Example: services.AddSingleton<IFileService, LocalFileService>();
                }

                // Configure logging services
                if (loggingService == "aws")
                {
                    // Configure AWS logging when needed
                    // Example: services.AddLogging(builder => builder.AddAWSProvider());
                }
            }
        }

        public class ConfigurationManager
        {
            public static IConfiguration Configuration { get; set; }
        }
    }