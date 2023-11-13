using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.Models;
using System.Security.Claims;

namespace BlogMVC.BLL.Services.BlogPostService
{
    public interface IBlogPostService
    {
        Task<BlogPost> CreateNewBlogPost(CreateBlogPostDTO request, int categoryId);

        Task DeleteBlogPost(int id);

        Task EditBlogPost(EditBlogPostDTO request);

        Task<IEnumerable<BlogPostDTO>> GetAllBlogPosts(BlogPostSearchParametersDTO request);

        Task<AuthorDTO> GetAuthorByUser(ClaimsPrincipal request);

        Task<BlogPostAndCategoryNameDTO> GetBlogPostAndCategoryName(int? id);

        Task<BlogPostWithCommentsDTO> GetBlogPostById(int? id);

        Task<int> GetCategoryId(string categoryName);

        Task<BlogPostDTO> SimpleGetBlogPostById(int? id);
    }
}
