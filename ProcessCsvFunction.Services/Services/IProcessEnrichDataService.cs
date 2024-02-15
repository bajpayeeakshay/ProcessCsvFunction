using ProcessCsvFunction.Services.Models;

namespace ProcessCsvFunction.Services.Services;

public interface IProcessEnrichDataService
{
    Task<bool> EnrichCsvAsync(Stream csvStream, string? fileName);

    bool ValidateCsvHeaders(string csvStreamHeader);

    Task<IEnumerable<EnrichedDataResponse>> GetEnrichedDataAsync(string fileName);
}
