using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.Models;

namespace BlogMVC.BLL
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile() 
        {
            CreateMap<CreateBlogPostDTO, BlogPost>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.AuthorId))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Text, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.Text))
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(s => s.BlogPostCreateViewModel.Title))
                .ForMember(dst => dst.Author, opt => opt.Ignore())
                .ForMember(dst => dst.Category, opt => opt.Ignore());

            CreateMap<EditBlogPostDTO, BlogPost>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(s => s.CreateViewModel.AuthorId))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Text, opt => opt.MapFrom(s => s.CreateViewModel.Text))
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(s => s.CreateViewModel.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(s => s.CreateViewModel.Title))
                .ForMember(dst => dst.Author, opt => opt.Ignore())
                .ForMember(dst => dst.Category, opt => opt.Ignore());

            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostCreateDTO>();
        }
    }
}
