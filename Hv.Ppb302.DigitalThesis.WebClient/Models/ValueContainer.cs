using System.Text.Json.Serialization;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class ValueContainer
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
