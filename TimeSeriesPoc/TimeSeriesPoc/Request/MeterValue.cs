namespace TimeSeriesPoc.Request;

public class MeterValue
{
    public DateTimeOffset Timestamp { get; set; }
    public List<SampledValue> SampledValue { get; set; } = default!;
}