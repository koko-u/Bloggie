using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Bloggie.Web.Extensions;

public static class TempDataExtensions
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        where T : class
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    public static T? Get<T>(this ITempDataDictionary tempData, string key)
        where T : class
    {
        if (tempData.TryGetValue(key, out var obj))
        {
            if (obj is string value)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
        }

        return null;
    }
}
