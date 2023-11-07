using BlogMVC.DAL.Models;
using MediatR;

namespace BlogMVC.BLL.AuthorsOperations.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<Unit>
    {
        public Author Author { get; set; }
    }
}
