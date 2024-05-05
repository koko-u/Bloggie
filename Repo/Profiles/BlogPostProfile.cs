using AutoMapper;
using Bloggie.Db.Models.Domain;
using Bloggie.Repo.Models.ViewModels;

namespace Bloggie.Repo.Profiles;

public class BlogPostProfile : Profile
{
    public BlogPostProfile()
    {
        CreateMap<AddBlogPost, BlogPost>()
            .ForMember(dest => dest.Heading, opt => opt.PreCondition(src => src.Heading is not null))
            .ForMember(dest => dest.PageTitle, opt => opt.PreCondition(src => src.PageTitle is not null))
            .ForMember(dest => dest.Content, opt => opt.PreCondition(src => src.Content is not null))
            .ForMember(dest => dest.ShortDescription, opt => opt.PreCondition(src => src.ShortDescription is not null))
            .ForMember(dest => dest.FeaturedImageUrl, opt => opt.PreCondition(src => src.FeaturedImageUrl is not null))
            .ForMember(dest => dest.UrlHandle, opt => opt.PreCondition(src => src.UrlHandle is not null))
            .ForMember(dest => dest.PublishedOn, opt => opt.PreCondition(src => src.PublishedOn is not null))
            .ForMember(dest => dest.Author, opt => opt.PreCondition(src => src.Author is not null))
            ;

        CreateMap<BlogPost, BlogPostRow>();

        CreateMap<BlogPostRow, BlogPost>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Heading, opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.Heading)))
            .ForMember(dest => dest.PageTitle, opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.PageTitle)))
            .ForMember(dest => dest.Content, opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.Content)))
            .ForMember(dest => dest.ShortDescription,
                opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.ShortDescription)))
            .ForMember(dest => dest.FeaturedImageUrl,
                opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.FeaturedImageUrl)))
            .ForMember(dest => dest.UrlHandle, opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.UrlHandle)))
            .ForMember(dest => dest.PublishedOn, opt => opt.PreCondition(src => src.PublishedOn is not null))
            .ForMember(dest => dest.Author, opt => opt.PreCondition(src => !string.IsNullOrEmpty(src.Author)))
            ;
    }
}
