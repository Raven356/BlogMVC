using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostsById
{
    public class GetBlogPostsByIdRequestHandler : IRequestHandler<GetBlogPostsByIdRequest, BlogPostWithComments>
    {
        private readonly BlogMVCContext _context;

        public GetBlogPostsByIdRequestHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<BlogPostWithComments> Handle(GetBlogPostsByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _context.BlogPost
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            blogPost.Author = await _context.Author.FindAsync(blogPost.AuthorId);
            blogPost.Category = await _context.Category.FindAsync(blogPost.CategoryId);

            var comments = await _context.Comment.Where(c => c.BlogPostId == request.Id).ToListAsync();
            comments.ForEach(c => c.User = _context.User.Find(c.UserId));
            BlogPostWithComments blogPostWithComments =
                new BlogPostWithComments
                {
                    BlogPostValue = blogPost,
                    CommentList = comments,
                    NewComment = new Comment()
                };
            return blogPostWithComments;
        }
    }
}
