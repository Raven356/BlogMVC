using BlogMVC.BLL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostsById
{
    public class GetBlogPostsByIdRequest : IRequest<BlogPostWithComments>
    {
        public int? Id { get; set; }
    }
}
