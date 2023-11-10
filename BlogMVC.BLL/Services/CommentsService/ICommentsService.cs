using BlogMVC.BLL.Models;

namespace BlogMVC.BLL.Services.ControllersService
{
    public interface ICommentsService
    {
        public Task<BlogPostWithCommentsDTO> AddNewComment(BlogPostWithCommentsDTO request);
    }
}
