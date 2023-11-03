using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.CategoriesOperations
{
    public class GetCategoriesByIdRequestHandler : IRequestHandler<GetCategoriesByIdRequest, Category>
    {
        private readonly BlogMVCContext _context;

        public GetCategoriesByIdRequestHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<Category> Handle(GetCategoriesByIdRequest request, CancellationToken cancellationToken)
        {
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            return category;
        }
    }
}
