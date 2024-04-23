namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<MolecularMosaic> MolecularMosaics { get; set; } = [];
        public List<MolarMosaics> MolarMosaics { get; set; } = [];
    }
}
