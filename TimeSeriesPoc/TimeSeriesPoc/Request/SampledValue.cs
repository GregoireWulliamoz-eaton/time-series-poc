namespace TimeSeriesPoc.Request;

public class SampledValue
{
    public string Value { get; set; } = default!;
    public string Context { get; set; } = "Sample.Periodic";
    public string Format { get; set; } = "Raw";
    public string Measurand { get; set; } = "Energy.Active.Import.Register";
    public string? Phase { get; set; }
    public string Location { get; set; } = "Outlet";
    public string? Unit { get; set; } = "Wh";
}