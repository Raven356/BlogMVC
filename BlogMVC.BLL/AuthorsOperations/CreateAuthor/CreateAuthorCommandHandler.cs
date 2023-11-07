using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;

namespace BlogMVC.BLL.AuthorsOperations.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Unit>
    {
        private readonly IRepository<Author> _repository;

        public CreateAuthorCommandHandler(IRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            await _repository.Add(request.Author);
            return Unit.Value;
        }
    }
}
