using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using ProcessCsvFunction.Data.Abstraction;
using ProcessCsvFunction.Data.Models;
using Serilog;

namespace ProcessCsvFunction.Data.Repository;

public class EnrichedDataSetRepository : IEnrichedDataSetRepository
{
    internal TableClient _tableClient { get; set; }
    private readonly ILogger _logger;

    public EnrichedDataSetRepository(IOptions<AzureConfig> options, ILogger logger)
    {
        _logger = logger;
        var tableServiceClient = new TableServiceClient(options.Value.ConnectionString);
        tableServiceClient.CreateTableIfNotExists(options.Value.TableStorageName);
        _tableClient = tableServiceClient.GetTableClient(options.Value.TableStorageName);
    }

    public async Task<IEnumerable<EnrichedDataSet>> GetEnrichedDataSetFromFileNameAsync(string fileName)
    {
        var result = new List<EnrichedDataSet>();
        await foreach (var data in _tableClient.QueryAsync<EnrichedDataSet>(t => t.FileName == fileName))
        {
            result.Add(data);
        }

        return result;
    }

    public async Task<bool> SaveEnrichedDataSetAsync(EnrichedDataSet enrichedDataSets)
    {
        await _tableClient.AddEntityAsync(enrichedDataSets);

        return true;
    }
}
