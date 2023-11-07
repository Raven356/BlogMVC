using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.BlogPostOperations.SimpleGetById
{
    public class SimpleGetByIdRequestHandler : IRequestHandler<SimpleGetByIdRequest, BlogPost>
    {
        private readonly IRepository<BlogPost> _repository;

        public SimpleGetByIdRequestHandler(IRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public async Task<BlogPost> Handle(SimpleGetByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _repository.GetAll().AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            return blogPost;
        }
    }
}
