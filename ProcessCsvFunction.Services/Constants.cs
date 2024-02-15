using System.ComponentModel;

namespace ProcessCsvFunction.Services;

public static class Constants
{
    public const int BatchSize = 10;
    public const int NumberOfFields = 8;
    public const string Delimiter = ",";
    public const string GleifUrlVarName = "Gleif_Url";
    public const string TableStorageVarName = "TableStorageName";
    public const string BlobStorageVarName = "BlobStorageName";
    public const string AzureStorageConnectionString = "AZURE_STORAGE_CONNECTION_STRING";
}

public enum CsvFields
{
    [Description("transaction_uti")]
    TransactionUti = 0,
    [Description("isin")]
    Isin = 1,
    [Description("notional")]
    Notional = 2,
    [Description("notional_currency")]
    NotionalCurrency = 3,
    [Description("transaction_type")]
    TransactionType = 4,
    [Description("transaction_datetime")]
    TransactionDatetime = 5,
    [Description("rate")]
    Rate = 6,
    [Description("lei")]
    Lei = 7
}
