using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class MolarMosaicCrudViewModel
    {
        public MolecularMosaic? MolarMosaic { get; set; }
        public IEnumerable<SelectListItem>? ConnectorTags { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem>? AssemblageTags { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem>? KaleidoscopeTags { get; set; } = Enumerable.Empty<SelectListItem>();
        public List<string> Becomings { get; set; } = new List<string>();
        
        public List<Guid>? SelectedConnectorsTagsIds { get; set; } = [];
        public List<Guid>? SelectedKaleidoscopeTagsIds { get; set; } = [];
        public Guid SelectedAssemblageId { get; set; }
        public List<string>? SelectedBecomings { get; set; } = [];

    }
}
