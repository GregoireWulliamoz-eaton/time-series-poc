using OpenSearch.Client;
using TimeSeriesPoc.Request;

namespace TimeSeriesPoc;

public class ChargingStationLogDto
{
    [PropertyName("fields")] public Fields Fields { get; set; } = default!;

    [PropertyName("ChargePointIdentifier")]
    public string ChargePointIdentifier { get; set; } = default!;
}

/// <summary>
///     OCPPj data
/// </summary>
public class Fields
{
    [PropertyName("OcppMessage")]
    public OcppMessage OcppMessage { get; set; } = default!;
}

public class OcppMessage
{
    [PropertyName("MessageType")]
    public string MessageType { get; set; } = default!;

    [PropertyName("Name")]
    public string Name { get; set; } = default!;

    [PropertyName("Payload")]
    public MeterValuesRequest Payload { get; set; } = default!;
}