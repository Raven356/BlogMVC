using BlogMVC.DAL.Models;
using MediatR;

namespace BlogMVC.BLL.AuthorsOperations.GetAuthorById
{
    public class GetAuthorByIdRequest : IRequest<Author>
    {
        public int? Id { get; set; }
    }
}
