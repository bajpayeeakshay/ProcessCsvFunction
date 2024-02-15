namespace ProcessCsvFunction.Services.Models;

public class EnrichedData
{
    public string? TransactionUti { get; set; }
    public string? Isin {  get; set; }
    public double Notional { get; set; }
    public string? NotionalCurrency { get; set; }
    public string? TransactionType { get; set; }
    public DateTime? TransactionDateTime { get; set; }
    public double Rate { get; set; }
    public string? Lei { get; set; }
    public LegalName? LegalNameValue { get; set; }
    public List<string>? Bic { get; set; }
    public double? TransactionCosts { get; set; }
}
