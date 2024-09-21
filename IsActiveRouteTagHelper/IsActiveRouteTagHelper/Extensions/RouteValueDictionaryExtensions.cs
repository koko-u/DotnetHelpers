namespace IsActiveRouteTagHelper.Extensions;

internal static class RouteValueDictionaryExtensions
{
    public static string GetStringValueOrDefault(this IDictionary<string, object?> dictionary, string key)
    {
        if (dictionary.TryGetValue(key, out var value))
        {
            if (value is string svalue)
            {
                return svalue;
            }
        }

        return string.Empty;
    }
}