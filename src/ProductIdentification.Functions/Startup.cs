using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductIdentification.Core.Models;
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
            
            var config = ConfigureAppSettings();
            services.AddSingleton(config);

            services.AddScoped<AzureStorageAccountFactory>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTrainingRepository, ProductTrainingRepository>();
            services.AddScoped<IFileRepository, AzureFileRepository>();

            services.AddScoped<IProductIdentifyService, ProductIdentifyService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IEmailService, EmailService>();
            
            services.AddScoped<ISecretsFetcher, KeyVaultSecretsFetcher>();
        }

        private static AppSettings ConfigureAppSettings()
        {
            var config = new AppSettings
            {
                KeyVaultUri = Environment.GetEnvironmentVariable(nameof(AppSettings.KeyVaultUri))
            };
            return config;
        }
    }
}