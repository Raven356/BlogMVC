using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.AddNewComment
{
    public class AddNewCommentHandler : IRequestHandler<AddNewCommentCommand, BlogPostWithComments>
    {
        private readonly BlogMVCContext _context;

        public AddNewCommentHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<BlogPostWithComments> Handle(AddNewCommentCommand request, CancellationToken cancellationToken)
        {
            await _context.AddAsync(request.BlogPostWithComments.NewComment);
            await _context.SaveChangesAsync();
            request.BlogPostWithComments.CommentList = await _context.Comment
                .Where(c => c.BlogPostId == request.BlogPostWithComments.BlogPostValue.Id).ToListAsync();
            request.BlogPostWithComments.CommentList.ForEach(c => c.User = _context.User.Find(c.UserId));
            return request.BlogPostWithComments;
        }
    }
}
