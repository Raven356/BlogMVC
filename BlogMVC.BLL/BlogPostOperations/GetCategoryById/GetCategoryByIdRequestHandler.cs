using BlogMVC.BLL.Context;
using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetCategoryById
{
    public class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, int>
    {
        private readonly BlogMVCContext _context;

        public GetCategoryByIdRequestHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
        {
            int categoryId = -1;
            while (categoryId == -1)
            {
                var category = _context.Category.Where(c => c.Name == request.BlogPostCreateViewModel.CategoryName).FirstOrDefault();

                if (category == null)
                {
                    await _context.AddAsync(new Category { Name = request.BlogPostCreateViewModel.CategoryName });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    categoryId = category.Id;
                }
            }
            return categoryId;
        }
    }
}
