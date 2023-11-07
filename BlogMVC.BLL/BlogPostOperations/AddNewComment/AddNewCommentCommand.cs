using BlogMVC.BLL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.AddNewComment
{
    public class AddNewCommentCommand : IRequest<BlogPostWithComments>
    {
        public BlogPostWithComments BlogPostWithComments { get; set; }
    }
}
