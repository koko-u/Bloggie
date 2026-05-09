using System;
using System.Collections.Generic;

namespace Bloggie.Web.Models.Domain;

/// <summary>
/// Blog Tag
/// </summary>
public sealed class Tag
{
    /// <summary>
    /// inner id of Tag
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Tag's name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Blog posts associated with this tag
    /// </summary>
    public List<BlogPost> BlogPosts { get; set; } = [];
}
