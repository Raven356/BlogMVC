using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.AuthorsOperations.GetAuthorById
{
    public class GetAuthorByIdRequestHandler : IRequestHandler<GetAuthorByIdRequest, Author>
    {
        private readonly IRepository<Author> _repository;
        private readonly IRepository<User> _userRepository;

        public GetAuthorByIdRequestHandler(IRepository<Author> repository, IRepository<User> userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<Author> Handle(GetAuthorByIdRequest request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetById(request.Id);

            author.User = await _userRepository.GetById(author.UserId);
            return author;
        }
    }
}
