using FunctionApp1.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]


namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //string cs = Environment.GetEnvironmentVariable("ConnectionString");
            // Microsoft.Extensions.Configuration.AzureAppConfiguration
            //builder.ConfigurationBuilder.AddAzureAppConfiguration(cs);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // GitHub 送信サービス追加
            builder.Services.AddTransient<GitHubIssueService, GitHubIssueService>();
        }
    }
}
