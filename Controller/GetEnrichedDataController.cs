using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using ProcessCsvFunction.Services.Services;
using Serilog;
using System.Threading.Tasks;

namespace ProcessCsvFunction.Controller;

public class EnrichedDataController
{
    private readonly IProcessEnrichDataService _processEnrichDataService;
    private readonly ILogger _logger;

    public EnrichedDataController(IProcessEnrichDataService processEnrichDataService, ILogger logger)
    {
        _processEnrichDataService = processEnrichDataService;
        _logger = logger.ForContext<EnrichedDataController>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [FunctionName(nameof(EnrichedDataController))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.Information("C# HTTP trigger function processed a request.");

        string fileName = req.Query["fileName"];

        var result = await _processEnrichDataService.GetEnrichedDataAsync(fileName);

        return new OkObjectResult(result);
    }
}
