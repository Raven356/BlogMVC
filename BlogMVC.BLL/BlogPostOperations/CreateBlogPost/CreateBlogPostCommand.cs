using BlogMVC.BLL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.CreateBlogPost
{
    public class CreateBlogPostCommand : IRequest<Unit>
    {
        public BlogPostCreateViewModel BlogPostCreateViewModel { get; set; }

        public int CategoryId { get; set; }
    }
}
