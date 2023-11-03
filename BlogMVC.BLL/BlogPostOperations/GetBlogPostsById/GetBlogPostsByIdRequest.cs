using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostsById
{
    public class GetBlogPostsByIdRequest : IRequest<BlogPostWithComments>
    {
        public int? Id { get; set; }
    }
}
