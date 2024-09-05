using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public static class Extensions
{

    public static IEnumerable<SelectListItem> ToSelectListItemsList<T>(
        this IEnumerable<T>? items,
        Func<T, string> valueSelector,
        Func<T, string?> textSelector,
        IEnumerable<string>? selectedValues = null)
    {
        var selectedValuesSet = selectedValues?.ToHashSet() ?? new HashSet<string>();

        return (items ?? Enumerable.Empty<T>()).Select(item => new SelectListItem
        {
            Value = valueSelector(item),
            Text = textSelector(item) ?? string.Empty,
            Selected = selectedValuesSet.Contains(valueSelector(item))
        }).ToList();
    }

    public static List<string> ToStringListFromTagifyFormat(this string? json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        var valueContainers = JsonSerializer.Deserialize<List<ValueContainer>>(json);
        return valueContainers?.Where(vc => vc.Value != null)
                   .Select(vc => vc.Value!)
                   .ToList()
               ?? [];
    }



}