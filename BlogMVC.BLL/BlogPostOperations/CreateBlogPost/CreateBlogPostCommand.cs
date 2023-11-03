using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.CreateBlogPost
{
    public class CreateBlogPostCommand : IRequest<Unit>
    {
        public BlogPostCreateViewModel BlogPostCreateViewModel { get; set; }

        public int CategoryId { get; set; }
    }
}
