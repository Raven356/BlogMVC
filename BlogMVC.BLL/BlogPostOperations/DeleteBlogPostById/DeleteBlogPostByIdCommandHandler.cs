using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById
{
    public class DeleteBlogPostByIdCommandHandler : IRequestHandler<DeleteBlogPostByIdCommand, Unit>
    {
        private readonly IRepository<BlogPost> _repository;

        public DeleteBlogPostByIdCommandHandler(IRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteBlogPostByIdCommand request, CancellationToken cancellationToken)
        {
            await _repository.Delete(request.Id);
            return Unit.Value;
        }
    }
}
