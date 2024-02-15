namespace ProcessCsvFunction.Services.Models;

public class EnrichedDataResponse
{
    public string FileName { get; set; }

    public DateTime ProcessedDateTime { get; set; }

    public IEnumerable<EnrichedData>? EnrichedDatas { get; set; }
}
