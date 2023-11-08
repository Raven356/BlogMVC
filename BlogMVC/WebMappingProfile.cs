using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.Models;

namespace BlogMVC
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile() 
        { 
            CreateMap<RegisterViewModel, UserDTO>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(rm => rm.Email));
            CreateMap<LoginViewModel, LoginDTO>();
        }
    }
}
