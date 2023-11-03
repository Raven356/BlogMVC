using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.EditBlogPost
{
    public class EditBlogPostCommandHandler : IRequestHandler<EditBlogPostCommand, Unit>
    {
        private readonly BlogMVCContext _context;

        public EditBlogPostCommandHandler(BlogMVCContext blogMVCContext)
        {
            _context = blogMVCContext;
        }

        public async Task<Unit> Handle(EditBlogPostCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var blog = await _context.BlogPost.FindAsync(request.CreateViewModel.Id);
                blog.AuthorId = request.CreateViewModel.AuthorId;
                blog.CategoryId = request.CategoryId;
                blog.Text = request.CreateViewModel.Text;
                blog.Date = request.CreateViewModel.Date;
                blog.Title = request.CreateViewModel.Title;
                _context.Update(blog);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
            return Unit.Value;
        }
    }
}
