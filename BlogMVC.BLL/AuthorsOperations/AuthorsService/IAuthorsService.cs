using BlogMVC.BLL.AuthorsOperations.CreateAuthor;
using BlogMVC.BLL.AuthorsOperations.GetAuthorById;
using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.AuthorsOperations.AuthorsService
{
    public interface IAuthorsService
    {
        Task CreateAuthor(CreateAuthorCommand request);

        Task<Author> GetAuthorById(GetAuthorByIdRequest request);
    }
}
