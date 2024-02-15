using Microsoft.Azure.WebJobs;
using ProcessCsvFunction.Services;
using ProcessCsvFunction.Services.Services;
using Serilog;
using System.IO;

namespace ProcessCsvFunction.BlobTrigger;

public class ProcessCsvBlobTrigger
{
    private readonly IProcessEnrichDataService _processCsv;
    private readonly ILogger _logger;
    public ProcessCsvBlobTrigger(IProcessEnrichDataService processCsv, ILogger logger)
    {
        _processCsv = processCsv;
        _logger = logger.ForContext<ProcessCsvBlobTrigger>();
    }

    [FunctionName(nameof(ProcessCsvBlobTrigger))]
    public void Run([BlobTrigger("process-csv-function-blob/{name}",
        Connection = "AZURE_STORAGE_CONNECTION_STRING")]Stream myBlob,
        string name)
    {
        _logger.Information($"Blob Trigger function processing started for file - {name} ");
        _processCsv.EnrichCsvAsync(myBlob, name);
        _logger.Information($"Blob Trigger function processing completed for file - {name}");
    }
}
