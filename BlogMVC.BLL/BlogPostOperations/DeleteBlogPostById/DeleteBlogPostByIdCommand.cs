using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById
{
    public class DeleteBlogPostByIdCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
