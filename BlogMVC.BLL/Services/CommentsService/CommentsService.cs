﻿using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.ControllersService
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IMapper _mapper;

        public CommentsService(IRepository<Comment> commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task AddNewComment(CommentDTO newComment)
        {
            await _commentRepository.Add(_mapper.Map<Comment>(newComment));
        }
    }
}
