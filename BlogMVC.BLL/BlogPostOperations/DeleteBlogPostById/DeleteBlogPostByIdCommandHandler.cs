using BlogMVC.BLL.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById
{
    public class DeleteBlogPostByIdCommandHandler : IRequestHandler<DeleteBlogPostByIdCommand, Unit>
    {
        private readonly BlogMVCContext _context;

        public DeleteBlogPostByIdCommandHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteBlogPostByIdCommand request, CancellationToken cancellationToken)
        {
            var blogPost = await _context.BlogPost.FindAsync(request.Id);
            if (blogPost != null)
            {
                _context.BlogPost.Remove(blogPost);
            }

            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
