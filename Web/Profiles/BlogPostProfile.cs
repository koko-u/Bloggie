using AutoMapper;
using Bloggie.Db.Models.Domain;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Profiles;

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
    }
}
