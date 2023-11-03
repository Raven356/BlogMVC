using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
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
        private readonly BlogMVCContext _context;

        public GetAuthorByIdRequestHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<Author> Handle(GetAuthorByIdRequest request, CancellationToken cancellationToken)
        {
            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            author.User = await _context.User.FindAsync(author.UserId);
            return author;
        }
    }
}
