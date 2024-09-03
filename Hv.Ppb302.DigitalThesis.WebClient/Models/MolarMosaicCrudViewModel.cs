using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class MolarMosaicCrudViewModel
    {
        public MolarMosaic? MolarMosaic { get; set; } 
        public IEnumerable<SelectListItem>? ConnectorTags { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem>? AssemblageTags { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem>? KaleidoscopeTags { get; set; } = Enumerable.Empty<SelectListItem>();
        public string? Becomings { get; set; }
        public List<Guid> SelectedConnectorsTagsIds { get; set; } = [];
        public List<Guid> SelectedKaleidoscopeTagsIds { get; set; } = [];

    }
}
