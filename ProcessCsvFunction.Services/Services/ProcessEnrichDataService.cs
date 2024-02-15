using ProcessCsvFunction.Data.Abstraction;
using ProcessCsvFunction.Services.Extensions;
using ProcessCsvFunction.Services.Models;
using Serilog;

namespace ProcessCsvFunction.Services.Services;

public class ProcessEnrichDataService : IProcessEnrichDataService
{
    private readonly ILogger _logger;
    private readonly IGleifService _gleifService;
    private readonly IEnrichedDataSetRepository _enrichedDataSetRepository;

    public ProcessEnrichDataService(ILogger logger,
        IGleifService gleifService,
        IEnrichedDataSetRepository enrichedDataSetRepository)
    {
        _logger = logger;
        _gleifService = gleifService;
        _enrichedDataSetRepository = enrichedDataSetRepository;
    }

    public async Task<bool> EnrichCsvAsync(Stream csvStream, string? fileName)
    {
        var dataList = new List<EnrichedData>();
        try
        {
            using (var reader = new StreamReader(csvStream))
            {
                if (ValidateCsvHeaders(reader.ReadLine()))
                {
                    while (!reader.EndOfStream)
                    {
                        var batchValues = await CompileDataInBatchesAsync(reader);
                        await batchValues.EnrichWithGleifValuesAsync(_gleifService);
                        dataList.AddRange(batchValues);
                    }
                }
                else
                {
                    _logger.Error($"Invalid File Received: {fileName}");
                    return false;
                }
            }

            return await _enrichedDataSetRepository.SaveEnrichedDataSetAsync(dataList.ToEnrichedDataSet(fileName));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error Occurred while processing request");

            return false;
        }
    }

    public async Task<IEnumerable<EnrichedData>> CompileDataInBatchesAsync(StreamReader reader)
    {
        List<EnrichedData> batchValues = new List<EnrichedData>();
        try
        {
            for (int i = 0; i < Constants.BatchSize && !reader.EndOfStream; i++)
            {
                string? line = await reader.ReadLineAsync();
                string[]? values = line?.Split(Constants.Delimiter);
                if (values?.Length == 8)
                {
                    var val = new EnrichedData
                    {
                        TransactionUti = values[(int)CsvFields.TransactionUti],
                        Isin = values[(int)CsvFields.Isin],
                        Notional = Convert.ToDouble(values[(int)CsvFields.Notional]),
                        NotionalCurrency = values[(int)CsvFields.NotionalCurrency],
                        TransactionType = values[(int)CsvFields.TransactionType],
                        TransactionDateTime = DateTime.Parse(values[(int)CsvFields.TransactionDatetime]),
                        Rate = Convert.ToDouble(values[(int)CsvFields.Rate]),
                        Lei = values[(int)CsvFields.Lei]
                    };
                    batchValues.Add(val);
                }
                else
                {
                    _logger.Error($"Invalid data received: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while Compiling Data in Batches.");
        }
        return batchValues;
    }

    public async Task<IEnumerable<EnrichedDataResponse>> GetEnrichedDataAsync(string fileName)
    {
        try
        {
            var enrichedDataSet = await _enrichedDataSetRepository.GetEnrichedDataSetFromFileNameAsync(fileName);
            return enrichedDataSet.ToEnrichedDataResponse();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error Occurred while fetching records for file: {fileName}");
        }

        return default;
    }

    public bool ValidateCsvHeaders(string csvStreamHeader)
    {
        var headers = csvStreamHeader?.Split(Constants.Delimiter);

        return headers != null && headers.Length == 8
            && headers[(int)CsvFields.TransactionUti] == CsvFields.TransactionUti.GetDescription()
            && headers[(int)CsvFields.Isin] == CsvFields.Isin.GetDescription()
            && headers[(int)CsvFields.Notional] == CsvFields.Notional.GetDescription()
            && headers[(int)CsvFields.NotionalCurrency] == CsvFields.NotionalCurrency.GetDescription()
            && headers[(int)CsvFields.TransactionType] == CsvFields.TransactionType.GetDescription()
            && headers[(int)CsvFields.TransactionDatetime] == CsvFields.TransactionDatetime.GetDescription()
            && headers[(int)CsvFields.Rate] == CsvFields.Rate.GetDescription()
            && headers[(int)CsvFields.Lei] == CsvFields.Lei.GetDescription();
    }
}
