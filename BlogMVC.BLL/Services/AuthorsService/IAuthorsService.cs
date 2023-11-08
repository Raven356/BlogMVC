using BlogMVC.Models;

namespace BlogMVC.BLL.Services.AuthorsService
{
    public interface IAuthorsService
    {
        Task CreateAuthor(AuthorDTO request);

        Task<AuthorDTO> GetAuthorById(int? request);
    }
}
