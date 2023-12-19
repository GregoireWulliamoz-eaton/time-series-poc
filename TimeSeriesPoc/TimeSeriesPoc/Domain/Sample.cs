namespace TimeSeriesPoc.Domain;

public class Sample
{
    public string ChargerId { get; set; } = default!;
    public int ConnectorId { get; set; }
    public int? TransactionId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Value { get; set; } = default!;
    public string Context { get; set; } = default!;
    public string Format { get; set; } = default!;
    public string Measurand { get; set; } = default!;
    public string? Phase { get; set; }
    public string Location { get; set; } = default!;
    public string? Unit { get; set; }
}