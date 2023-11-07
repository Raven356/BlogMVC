using BlogMVC.BLL.AuthorsOperations.CreateAuthor;
using BlogMVC.BLL.AuthorsOperations.GetAuthorById;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;

namespace BlogMVC.BLL.AuthorsOperations.AuthorsService
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IRepository<Author> _repository;
        private readonly IRepository<User> _userRepository;

        public AuthorsService(IRepository<Author> repository, IRepository<User> userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task CreateAuthor(CreateAuthorCommand request)
        {
            await _repository.Add(request.Author);
            return ;
        }

        public async Task<Author> GetAuthorById(GetAuthorByIdRequest request)
        {
            var author = await _repository.GetById(request.Id);

            author.User = await _userRepository.GetById(author.UserId);
            return author;
        }
    }
}
