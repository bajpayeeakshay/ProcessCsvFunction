using ProcessCsvFunction.Data.Models;

namespace ProcessCsvFunction.Data.Abstraction;

public interface IEnrichedDataSetRepository
{
    Task<bool> SaveEnrichedDataSetAsync(EnrichedDataSet enrichedDataSets);

    Task<IEnumerable<EnrichedDataSet>> GetEnrichedDataSetFromFileNameAsync(string fileName);
}
