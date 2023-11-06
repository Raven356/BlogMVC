using AutoMapper;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.CreateBlogPost
{
    public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand, Unit>
    {
        private readonly IRepository<BlogPost> _repository;
        private readonly IMapper _mapper;

        public CreateBlogPostCommandHandler(IRepository<BlogPost> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
        {
            await _repository.Add(_mapper.Map<BlogPost>(request));
            return Unit.Value;
        }
    }
}
