using System;
using Bloggie.Web.Services;
using FluentValidation;

namespace Bloggie.Web.Models.Forms;

public sealed class AddBlogFormValidator : AbstractValidator<AddBlogForm>
{
    public AddBlogFormValidator(BlogPostsService blogPostsService)
    {
        RuleFor(form => form.Heading).NotEmpty().MaximumLength(255);

        RuleFor(form => form.PageTitle).NotEmpty().MaximumLength(255);

        RuleFor(form => form.ShortDescription).MaximumLength(255);

        RuleFor(form => form.FeaturedImageUrl).MaximumLength(255);

        RuleFor(form => form.Slug)
            .NotEmpty()
            .MaximumLength(255)
            .MustAsync(
                async (slug, cancellationToken) =>
                {
                    var blogPost = await blogPostsService.GetBySlugAsync(slug, cancellationToken);
                    return blogPost is null;
                }
            );

        RuleFor(form => form.PublishedDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("{PropertyName} cannot be in the future");

        RuleFor(form => form.Author).NotEmpty().MaximumLength(255);
    }
}
