using System;

namespace Bloggie.Web.Models.Rows;

public class TagRow
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
