using Newtonsoft.Json;
using ProcessCsvFunction.Services.Models;
using Serilog;

namespace ProcessCsvFunction.Services.Services;

public class GleifService : IGleifService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GleifService(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LeiRecordRoot?> GetLeiRecordByLeiAsync(IEnumerable<string> leis)
    {
        var remainingUrl = $"lei-records?filter[lei]={string.Join(",", leis)}";
        try
        {
            HttpResponseMessage response = _httpClient.GetAsync(remainingUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseResultString = await response.Content.ReadAsStringAsync();
                var responseResult = JsonConvert.DeserializeObject<LeiRecordRoot>(responseResultString);
                return responseResult;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting records fromm GLEIF");
        }
        return null;
    }
}
