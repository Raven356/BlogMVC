using BlogMVC.BLL.BlogPostOperations.AddNewComment;
using BlogMVC.BLL.BlogPostOperations.CreateBlogPost;
using BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById;
using BlogMVC.BLL.BlogPostOperations.EditBlogPost;
using BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts;
using BlogMVC.BLL.BlogPostOperations.GetAuthorIdByUser;
using BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById;
using BlogMVC.BLL.BlogPostOperations.GetBlogPostsById;
using BlogMVC.BLL.BlogPostOperations.GetCategoryById;
using BlogMVC.BLL.BlogPostOperations.SimpleGetById;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.BlogPostOperations.BlogPostService
{
    public interface IBlogPostService
    {
        Task<BlogPostWithComments> AddNewComment(AddNewCommentCommand request);

        Task CreateNewBlogPost(CreateBlogPostCommand request);

        Task DeleteBlogPost(DeleteBlogPostByIdCommand request);

        Task EditBlogPost(EditBlogPostCommand request);

        Task<List<BlogPost>> GetAllBlogPosts(GetBlogPostsRequest request);

        Task<Author> GetAuthorByUserId(GetUserAuthorByUserId request);

        Task<GetBlogPostAndCategoryNameByIdResponse> 
            GetBlogPostAndCategoryName(GetBlogPostAndCategoryNameByIdRequest request);

        Task<BlogPostWithComments> GetBlogPostById(GetBlogPostsByIdRequest request);

        Task<int> GetCategoryById(GetCategoryByIdRequest request);

        Task<BlogPost> SimpleGetBlogPostById(SimpleGetByIdRequest request);
    }
}
