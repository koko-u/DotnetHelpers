using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IsActiveRouteTagHelper.Extensions;

internal static class AttributesExtensions
{
    public static string[] GetClassAttributes(this ReadOnlyTagHelperAttributeList attributeList)
    {
        if (attributeList.TryGetAttribute("class", out var classAttrs))
        {
            return classAttrs.Value.ToString()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];
        }

        return [];
    }
}