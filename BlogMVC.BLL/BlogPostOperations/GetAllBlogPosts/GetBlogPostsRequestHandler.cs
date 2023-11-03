using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts
{
    public class GetBlogPostsRequestHandler : IRequestHandler<GetBlogPostsRequest, List<BlogPost>>
    {
        private readonly BlogMVCContext _context;
        private readonly UserManager<User> _userManager;

        public GetBlogPostsRequestHandler(BlogMVCContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<BlogPost>> Handle(GetBlogPostsRequest request, CancellationToken cancellationToken)
        {
            var blogs = _context.BlogPost.Select(b => b);

            if (!string.IsNullOrEmpty(request.SearchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrEmpty(request.SearchCategory))
            {
                blogs = blogs.Where(b => _context.Category.Find(b.CategoryId).Name.Contains(request.SearchCategory));
            }

            if (!string.IsNullOrEmpty(request.SearchAuthor))
            {
                blogs = blogs.Where(b => _context.Author.Find(b.AuthorId).NickName.Contains(request.SearchAuthor));
            }

            await blogs.ForEachAsync(b => b.Author = _context.Author.Find(b.AuthorId));
            await blogs.ForEachAsync(b => b.Category = _context.Category.Find(b.CategoryId));

            return await blogs.OrderBy(b => b.Title).ToListAsync();
        }
    }
}
