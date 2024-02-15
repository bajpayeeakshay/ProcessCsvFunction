using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ProcessCsvFunction.Data.Abstraction;
using ProcessCsvFunction.Data.Models;
using ProcessCsvFunction.Data.Repository;
using ProcessCsvFunction.Services;
using ProcessCsvFunction.Services.Services;
using Serilog;
using System;
using System.Net.Http.Headers;

[assembly: FunctionsStartup(typeof(ProcessCsvFunction.Startup))]
namespace ProcessCsvFunction;

public class Startup : FunctionsStartup
{
    public Startup() { }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var gleifUrl = Environment.GetEnvironmentVariable(Constants.GleifUrlVarName);

        var logger = new Serilog.LoggerConfiguration()
            .WriteTo.File($"Logs/{nameof(ProcessCsvFunction)}.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Serilog.Log.Logger = logger;
        builder.Services.AddSingleton<ILogger>(logger);
        builder.Services.AddTransient<IProcessEnrichDataService, ProcessEnrichDataService>();
        builder.Services.AddTransient<IGleifService, GleifService>();
        builder.Services.AddScoped<IEnrichedDataSetRepository, EnrichedDataSetRepository>();
        builder.Services.AddOptions<AzureConfig>().Configure(t =>
        {
            t.TableStorageName = Environment.GetEnvironmentVariable(Constants.TableStorageVarName);
            t.BlobStorageName = Environment.GetEnvironmentVariable(Constants.BlobStorageVarName);
            t.ConnectionString = Environment.GetEnvironmentVariable(Constants.AzureStorageConnectionString);
        });

        builder.Services.AddHttpClient<IGleifService, GleifService>("GLEIF", httpClient =>
        {
            httpClient.BaseAddress = new Uri(gleifUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
    }
}
