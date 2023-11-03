using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts
{
    public class GetBlogPostsRequest : IRequest<List<BlogPost>>
    {
        public string? SearchTitle { get; set; }

        public string? SearchCategory { get; set; }

        public string? SearchAuthor { get; set; }
    }
}
