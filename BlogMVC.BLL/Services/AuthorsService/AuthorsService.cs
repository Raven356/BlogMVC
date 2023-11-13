﻿using AutoMapper;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;

namespace BlogMVC.BLL.Services.AuthorsService
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IRepository<Author> _repository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthorsService(IRepository<Author> repository, IRepository<User> userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateAuthor(AuthorDTO request)
        {
            var author = _mapper.Map<Author>(request);
            await _repository.Add(author);
            return;
        }

        public async Task<AuthorDTO> GetAuthorById(int? id)
        {
            var author = await _repository.GetById(id);

            author.User = await _userRepository.GetById(author.UserId);

            var result = _mapper.Map<AuthorDTO>(author);
            return result;
        }
    }
}
