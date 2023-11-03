using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.AddNewComment
{
    public class AddNewCommentCommand : IRequest<BlogPostWithComments>
    {
        public BlogPostWithComments BlogPostWithComments { get; set; }
    }
}
