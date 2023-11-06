using BlogMVC.DAL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.SimpleGetById
{
    public class SimpleGetByIdRequest : IRequest<BlogPost>
    {
        public int? Id { get; set; }
    }
}
