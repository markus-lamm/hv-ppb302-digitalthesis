using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class MolecularMosaicCrudViewModel
    {
        public MolecularMosaic? MolecularMosaic { get; set; }
        public IEnumerable<SelectListItem>? ConnectorTagsItemList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem>? AssemblageTagsItemList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem>? KaleidoscopeTagsItemList { get; set; } = Enumerable.Empty<SelectListItem>();
        public string? Becomings { get; set; }
        public List<Guid> SelectedConnectorsTagsIds { get; set; } = [];
        public List<Guid> SelectedKaleidoscopeTagsIds { get; set; } = [];
    }
}
