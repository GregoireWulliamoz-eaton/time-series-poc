namespace TimeSeriesPoc.Request;

public class MeterValuesRequest
{
    public int ConnectorId { get; set; }
    public int? TransactionId { get; set; }
    public List<MeterValue> MeterValue { get; set; } = default!;
}