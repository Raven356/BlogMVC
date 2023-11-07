using BlogMVC.DAL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts
{
    public class GetBlogPostsRequest : IRequest<List<BlogPost>>
    {
        public string? SearchTitle { get; set; }

        public string? SearchCategory { get; set; }

        public string? SearchAuthor { get; set; }
    }
}
