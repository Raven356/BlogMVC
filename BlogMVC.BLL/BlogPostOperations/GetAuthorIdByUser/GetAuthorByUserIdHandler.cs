using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
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
        private readonly BlogMVCContext _context;
        private readonly UserManager<User> _userManager;

        public GetAuthorByUserIdHandler(BlogMVCContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Author> Handle(GetUserAuthorByUserId request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.User);
            string userId = user.Id;
            var author = await _context.Author.Where(a => a.UserId.Equals(userId)).FirstOrDefaultAsync();
            return author;
        }
    }
}
