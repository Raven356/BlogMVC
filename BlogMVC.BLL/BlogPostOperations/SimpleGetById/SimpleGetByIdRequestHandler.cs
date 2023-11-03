using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.SimpleGetById
{
    public class SimpleGetByIdRequestHandler : IRequestHandler<SimpleGetByIdRequest, BlogPost>
    {
        private readonly BlogMVCContext _context;

        public SimpleGetByIdRequestHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<BlogPost> Handle(SimpleGetByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _context.BlogPost
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            return blogPost;
        }
    }
}
