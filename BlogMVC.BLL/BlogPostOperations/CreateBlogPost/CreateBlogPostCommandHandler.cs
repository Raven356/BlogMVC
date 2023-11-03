using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.CreateBlogPost
{
    public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand, Unit>
    {
        private readonly BlogMVCContext _context;

        public CreateBlogPostCommandHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                await _context.AddAsync(new BlogPost
                {
                    AuthorId = request.BlogPostCreateViewModel.AuthorId
                    ,
                    CategoryId = request.CategoryId,
                    Date = request.BlogPostCreateViewModel.Date
                    ,
                    Text = request.BlogPostCreateViewModel.Text,
                    Title = request.BlogPostCreateViewModel.Title
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Unit.Value;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }
}
