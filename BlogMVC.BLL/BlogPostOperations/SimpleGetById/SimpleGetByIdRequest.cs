using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.SimpleGetById
{
    public class SimpleGetByIdRequest : IRequest<BlogPost>
    {
        public int? Id { get; set; }
    }
}
