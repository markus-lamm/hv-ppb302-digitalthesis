using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public static class Extensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(
            this IEnumerable<T>? items,
            Func<T, string> valueSelector,
            Func<T, string?> textSelector,
            string? selectedValue = null)
        {
            // Use null-coalescing operator to handle null items
            return (items ?? Enumerable.Empty<T>()).Select(item => new SelectListItem
            {
                Value = valueSelector(item),
                Text = textSelector(item),
                Selected = selectedValue != null && valueSelector(item) == selectedValue
            }).ToList();
        }

    }
}
