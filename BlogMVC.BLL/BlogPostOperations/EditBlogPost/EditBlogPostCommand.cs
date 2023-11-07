using BlogMVC.BLL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.EditBlogPost
{
    public class EditBlogPostCommand : IRequest<Unit>
    {
        public BlogPostCreateViewModel CreateViewModel { get; set; }

        public int CategoryId { get; set; }
    }
}
