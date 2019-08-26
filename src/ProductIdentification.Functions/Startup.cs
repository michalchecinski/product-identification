using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Data;
using ProductIdentification.Data.Repositories;
using ProductIdentification.Infrastructure;

[assembly: FunctionsStartup(typeof(ProductIdentification.Functions.Startup))]
namespace ProductIdentification.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var sqlConnection = Environment.GetEnvironmentVariable("DBConnection");
            services.AddDbContext<ProductIdentificationContext>(
                options => options.UseSqlServer(sqlConnection));

            services.AddLogging();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IProductIdentifyService, ProductIdentifyService>();

            var config = new AppSettings
            {
                CustomVisionEndpoint = Environment.GetEnvironmentVariable(nameof(AppSettings.CustomVisionEndpoint)),
                CustomVisionPredictionId =
                    Environment.GetEnvironmentVariable(nameof(AppSettings.CustomVisionPredictionId)),
                CustomVisionPredictionKey =
                    Environment.GetEnvironmentVariable(nameof(AppSettings.CustomVisionPredictionKey)),
                CustomVisionProjectId = Environment.GetEnvironmentVariable(nameof(AppSettings.CustomVisionProjectId)),
                CustomVisionTrainingKey =
                    Environment.GetEnvironmentVariable(nameof(AppSettings.CustomVisionTrainingKey))
            };

            services.AddSingleton(config);

            //services.AddOptions();
            //services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        }
    }
}