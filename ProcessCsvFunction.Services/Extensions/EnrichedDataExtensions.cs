using Newtonsoft.Json;
using ProcessCsvFunction.Data.Models;
using ProcessCsvFunction.Services.Models;
using ProcessCsvFunction.Services.Services;

namespace ProcessCsvFunction.Services.Extensions;

public static class EnrichedDataExtensions
{

    public static async Task EnrichWithGleifValuesAsync(this IEnumerable<EnrichedData> enrichedDatas, IGleifService gleifService)
    {
        var batchLeis = enrichedDatas.Select(t => t.Lei).ToList();

        var leiRecordsForBatch = await gleifService.GetLeiRecordByLeiAsync(batchLeis);

        foreach (var enrichedData in enrichedDatas)
        {
            var leiRecord = leiRecordsForBatch?.Data?.First(t => t.Id == enrichedData.Lei);
            var country = leiRecord?.Attributes?.Entity?.LegalAddress?.Country;
            enrichedData.LegalNameValue = leiRecord?.Attributes?.Entity?.LegalName;
            enrichedData.Bic = leiRecord?.Attributes?.Bic;
            if (country != null && country == "GB")
            {
                enrichedData.TransactionCosts = enrichedData.Notional * enrichedData.Rate - enrichedData.Notional;
            }
            else if (country != null && country == "NL")
            {
                enrichedData.TransactionCosts = Math.Abs(enrichedData.Notional / enrichedData.Rate - enrichedData.Notional);
            }
        }
    }

    public static EnrichedDataSet ToEnrichedDataSet(this IEnumerable<EnrichedData> enrichedDatas, string fileName)
    {
        return new EnrichedDataSet
        {
            PartitionKey = "Constant",
            RowKey = Guid.NewGuid().ToString(),
            FileName = fileName,
            Timestamp = DateTime.Now,
            EnrichedData = JsonConvert.SerializeObject(enrichedDatas)
        };
    }

    public static IEnumerable<EnrichedDataResponse> ToEnrichedDataResponse(this IEnumerable<EnrichedDataSet> enrichedDataSet)
    {
        return enrichedDataSet.Where(x => x != null).Select(x => new EnrichedDataResponse
        {
            FileName = x.FileName,
            ProcessedDateTime = x.Timestamp.Value.UtcDateTime,
            EnrichedDatas = JsonConvert.DeserializeObject<IEnumerable<EnrichedData>>(x.EnrichedData)
        }).ToList();
    }
}
