using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById
{
    public class DeleteBlogPostByIdCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
