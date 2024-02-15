using ProcessCsvFunction.Services.Models;

namespace ProcessCsvFunction.Services.Services;

public interface IGleifService
{
    Task<LeiRecordRoot?> GetLeiRecordByLeiAsync(IEnumerable<string> leis);
}
