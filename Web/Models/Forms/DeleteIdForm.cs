using System;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Models.Forms;

/// <summary>
/// Id on Delete Form data
/// </summary>
public sealed class DeleteIdForm
{
    [FromForm(Name = "id")]
    public required Guid Id { get; set; }
}
