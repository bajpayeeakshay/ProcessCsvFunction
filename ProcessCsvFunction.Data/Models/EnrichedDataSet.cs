using Azure;
using Azure.Data.Tables;

namespace ProcessCsvFunction.Data.Models;

public class EnrichedDataSet : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string FileName { get; set; }
    public string EnrichedData { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
