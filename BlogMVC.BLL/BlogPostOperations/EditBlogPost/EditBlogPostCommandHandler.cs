using AutoMapper;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.BlogPostOperations.EditBlogPost
{
    public class EditBlogPostCommandHandler : IRequestHandler<EditBlogPostCommand, Unit>
    {
        private readonly IRepository<BlogPost> _repository;
        private readonly IMapper _mapper;

        public EditBlogPostCommandHandler(IRepository<BlogPost> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditBlogPostCommand request, CancellationToken cancellationToken)
        {
            var blog = await _repository.GetById(request.CreateViewModel.Id);
            _mapper.Map(request, blog);
            await _repository.Update(blog);
            return Unit.Value;
        }
    }
}
