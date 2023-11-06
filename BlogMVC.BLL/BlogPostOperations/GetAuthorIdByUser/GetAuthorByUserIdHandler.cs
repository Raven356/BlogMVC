using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetAuthorIdByUser
{
    public class GetAuthorByUserIdHandler : IRequestHandler<GetUserAuthorByUserId, Author>
    {
        private readonly IRepository<Author> _repository;
        private readonly UserManager<User> _userManager;

        public GetAuthorByUserIdHandler(IRepository<Author> repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<Author> Handle(GetUserAuthorByUserId request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.User);
            string userId = user.Id;
            var author = await _repository.GetAll().AsQueryable().Where(a => a.UserId.Equals(userId)).FirstOrDefaultAsync();
            return author;
        }
    }
}
