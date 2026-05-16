using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Rows;
using Riok.Mapperly.Abstractions;

namespace Bloggie.Web.Mappings;

/// <summary>
/// Query result TagRow Mappers
/// </summary>
[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class TagRowExtension
{
    /// <summary>
    /// TagRow to Tag Model Mapper
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public static partial Tag ToTagModel(this TagRow row);
}
