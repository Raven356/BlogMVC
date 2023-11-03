using BlogMVC.BLL.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById
{
    public class GetBlogPostAndCategoryNameByIdRequestHandler : IRequestHandler<GetBlogPostAndCategoryNameByIdRequest, GetBlogPostAndCategoryNameByIdResponse>
    {
        private readonly BlogMVCContext _context;
        public GetBlogPostAndCategoryNameByIdRequestHandler(BlogMVCContext context)
        {
            _context = context;
        }

        public async Task<GetBlogPostAndCategoryNameByIdResponse> Handle(GetBlogPostAndCategoryNameByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _context.BlogPost.FindAsync(request.Id);

            var categoryName = (await _context.Category.FindAsync(blogPost.CategoryId)).Name;

            return new GetBlogPostAndCategoryNameByIdResponse { BlogPost = blogPost, CategoryName = categoryName };
        }
    }
}
