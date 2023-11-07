using AutoMapper;
using BlogMVC.BLL.BlogPostOperations.CreateBlogPost;
using BlogMVC.BLL.BlogPostOperations.EditBlogPost;
using BlogMVC.DAL.Models;

namespace BlogMVC.BLL
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile() 
        {
            CreateMap<CreateBlogPostCommand, BlogPost>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.AuthorId))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Text, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.Text))
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.Title))
                .ForMember(dst => dst.Author, opt => opt.Ignore())
                .ForMember(dst => dst.Category, opt => opt.Ignore());

            CreateMap<EditBlogPostCommand, BlogPost>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(s => s.CreateViewModel.AuthorId))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Text, opt => opt.MapFrom(s => s.CreateViewModel.Text))
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(s => s.CreateViewModel.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(s => s.CreateViewModel.Title))
                .ForMember(dst => dst.Author, opt => opt.Ignore())
                .ForMember(dst => dst.Category, opt => opt.Ignore());
        }
    }
}
