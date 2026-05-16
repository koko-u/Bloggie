using System;
using Bloggie.Web.Services;
using FluentValidation;
using KozLibraries.JsonMessages;

namespace Bloggie.Web.Models.Forms;

/// <summary>
/// Validator for new BlogForm Data
/// </summary>
public sealed class AddBlogFormValidator : AbstractValidator<AddBlogForm>
{
    /// <summary>
    /// Define validation rules for Add or Edit BlogForm
    /// </summary>
    /// <param name="blogPostsService"></param>
    /// <param name="messages"></param>
    public AddBlogFormValidator(BlogPostsService blogPostsService, JsonMessageLocalizer messages)
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

        RuleFor(form => form.Slug)
            .NotEmpty()
            .WithMessage(_ =>
                messages.Format(
                    "Validation.Required",
                    new { PropertyName = messages.Get("BlogPost.Slug") }
                )
            )
            .MaximumLength(255)
            .WithMessage(_ =>
                messages.Format(
                    "Validation.MaximumLength",
                    new { PropertyName = messages.Get("BlogPost.Slug"), MaxLength = 255 }
                )
            )
            .MustAsync(
                async (slug, cancellationToken) =>
                {
                    var blogPost = await blogPostsService.GetBySlugAsync(slug, cancellationToken);
                    return blogPost is null;
                }
            )
            .WithMessage(_ =>
                messages.Format(
                    "Validation.Unique",
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
            );

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
