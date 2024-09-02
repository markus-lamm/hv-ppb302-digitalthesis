namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class MolecularMosaicCrudViewModel
    {
        public MolecularMosaic? MolecularMosaic { get; set; }
        public string[]? Becomings { get; set; }
        public List<ConnectorTag>? ConnectorTags { get; set; }
        public List<AssemblageTag>? AssemblageTags { get; set; }
        public List<KaleidoscopeTag>? KaleidoscopeTags { get; set; }
        public List<Guid> SelectedConnectorsTagsIds { get; set; } = [];
        public List<Guid> SelectedKaleidoscopeTagsIds { get; set; } = [];
        public Guid SelectedAssemblageId { get; set; }
        public List<string> SelectedBecomings { get; set; } = [];
    }
}
