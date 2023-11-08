using BlogMVC.BLL.Models;
using BlogMVC.Models;
using System.Security.Claims;

namespace BlogMVC.BLL.Services.BlogPostService
{
    public interface IBlogPostService
    {
        Task<BlogPostWithCommentsDTO> AddNewComment(BlogPostWithCommentsDTO request);

        Task CreateNewBlogPost(CreateBlogPostDTO request);

        Task DeleteBlogPost(int request);

        Task EditBlogPost(EditBlogPostDTO request);

        Task<List<BlogPostDTO>> GetAllBlogPosts(GetBlogPostsDTO request);

        Task<AuthorDTO> GetAuthorByUserId(ClaimsPrincipal request);

        Task<GetBlogPostAndCategoryNameByIdDTO> GetBlogPostAndCategoryName(int? request);

        Task<BlogPostWithCommentsDTO> GetBlogPostById(int? request);

        Task<int> GetCategoryById(BlogPostCreateDTO request);

        Task<BlogPostDTO> SimpleGetBlogPostById(int? request);
    }
}
