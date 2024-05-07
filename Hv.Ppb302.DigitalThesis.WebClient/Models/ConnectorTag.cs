namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class ConnectorTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public List<GeoTag> GeoTags { get; set; } = [];
    public List<MolecularMosaic> MolecularMosaics { get; set; } = [];
    public List<MolarMosaic> MolarMosaics { get; set; } = [];
    public List<KaleidoscopeMosaic> KaleidoscopeMosaics { get; set; } = [];
}