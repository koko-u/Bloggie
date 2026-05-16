using System;
using FastCloner.SourceGenerator.Shared;

namespace Bloggie.Web.Models.Domain;

/// <summary>
/// Blog Tag
/// </summary>
[FastClonerClonable]
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
}
