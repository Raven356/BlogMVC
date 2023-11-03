using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.AuthorsOperations.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Unit>
    {
        private readonly BlogMVCContext _context;

        public CreateAuthorCommandHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            _context.Add(request.Author);
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
