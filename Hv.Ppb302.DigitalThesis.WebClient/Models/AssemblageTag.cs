namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class AssemblageTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public List<MolecularMosaic> MolecularMosaics { get; set; } = [];
    public List<MolarMosaic> MolarMosaics { get; set; } = [];
}