using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.EditBlogPost
{
    public class EditBlogPostCommand : IRequest<Unit>
    {
        public BlogPostCreateViewModel CreateViewModel { get; set; }

        public int CategoryId { get; set; }
    }
}
