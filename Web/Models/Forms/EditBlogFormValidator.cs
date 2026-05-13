using System;
using Bloggie.Web.Services;
using FluentValidation;
using KozLibraries.JsonMessages;

namespace Bloggie.Web.Models.Forms;

/// <summary>
/// Validator for Edit BlogForm Data
/// </summary>
public sealed class EditBlogFormValidator : AbstractValidator<EditBlogForm>
{
    /// <summary>
    /// Define validation rules for Edit BlogForm
    /// </summary>
    /// <param name="blogPostsService"></param>
    /// <param name="messages"></param>
    public EditBlogFormValidator(BlogPostsService blogPostsService, JsonMessageLocalizer messages)
    {
        RuleFor(form => form.Heading)
            .NotEmpty()
            .WithMessage(_ =>
                messages.Format(
                    "Validation.Required",
                    new { PropertyName = messages.Get("BlogPost.Heading") }
                )
            )
            .MaximumLength(255)
            .WithMessage(_ =>
                messages.Format(
                    "Validation.MaximumLength",
                    new { PropertyName = messages.Get("BlogPost.Heading"), MaxLength = 255 }
                )
            );

        RuleFor(form => form.PageTitle)
            .NotEmpty()
            .WithMessage(_ =>
                messages.Format(
                    "Validation.Required",
                    new { PropertyName = messages.Get("BlogPost.PageTitle") }
                )
            )
            .MaximumLength(255)
            .WithMessage(_ =>
                messages.Format(
                    "Validation.MaximumLength",
                    new { PropertyName = messages.Get("BlogPost.PageTitle"), MaxLength = 255 }
                )
            );

        RuleFor(form => form.ShortDescription)
            .MaximumLength(255)
            .WithMessage(_ =>
                messages.Format(
                    "Validation.MaximumLength",
                    new
                    {
                        PropertyName = messages.Get("BlogPost.ShortDescription"),
                        MaxLength = 255,
                    }
                )
            );

        RuleFor(form => form.FeaturedImageUrl)
            .MaximumLength(255)
            .WithMessage(_ =>
                messages.Format(
                    "Validation.MaximumLength",
                    new
                    {
                        PropertyName = messages.Get("BlogPost.FeaturedImageUrl"),
                        MaxLength = 255,
                    }
                )
            );

        RuleFor(form => form.Slug)
            .MustAsync(
                async (form, slug, cancellationToken) =>
                {
                    var oldData = await blogPostsService.GetByIdAsync(form.Id, cancellationToken);
                    if (oldData is null)
                    {
                        // no blog post found, skip validation
                        return true;
                    }

                    // slug value cannot change
                    return string.Equals(oldData.Slug, slug, StringComparison.OrdinalIgnoreCase);
                }
            )
            .WithMessage(_ =>
                messages.Format(
                    "Validation.SlugCannotChange",
                    new { PropertyName = messages.Get("BlogPost.Slug") }
                )
            );

        RuleFor(form => form.PublishedDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(_ =>
                messages.Format(
                    "Validation.PastDate",
                    new { PropertyName = messages.Get("BlogPost.PublishedDate") }
                )
            )
            .MustAsync(
                async (form, publishedDate, cancellationToken) =>
                {
                    var oldData = await blogPostsService.GetByIdAsync(form.Id, cancellationToken);
                    if (oldData is null)
                    {
                        // no blog post found, skip validation
                        return true;
                    }

                    if (!oldData.PublishedDate.HasValue)
                    {
                        // old value has not published, allow change
                        return true;
                    }

                    // if already published, do not allow change
                    return publishedDate == oldData.PublishedDate;
                }
            )
            .WithMessage(_ => messages.Get("Validation.DoublePublished"));

        RuleFor(form => form.Author)
            .NotEmpty()
            .WithMessage(_ =>
                messages.Format(
                    "Validation.Required",
                    new { PropertyName = messages.Get("BlogPost.Author") }
                )
            )
            .MaximumLength(255)
            .WithMessage(_ =>
                messages.Format(
                    "Validation.MaximumLength",
                    new { PropertyName = messages.Get("BlogPost.Author"), MaxLength = 255 }
                )
            );
    }
}
