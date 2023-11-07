using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById
{
    public class GetBlogPostAndCategoryNameByIdRequestHandler : IRequestHandler<GetBlogPostAndCategoryNameByIdRequest,
        GetBlogPostAndCategoryNameByIdResponse>
    {
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Category> _categoryRepository;

        public GetBlogPostAndCategoryNameByIdRequestHandler(IRepository<BlogPost> blogPostRepository
            , IRepository<Category> categoryRepository )
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<GetBlogPostAndCategoryNameByIdResponse> Handle(GetBlogPostAndCategoryNameByIdRequest request,
            CancellationToken cancellationToken)
        {
            var blogPost = await _blogPostRepository.GetById(request.Id);

            var categoryName = (await _categoryRepository.GetById(blogPost.CategoryId)).Name;

            return new GetBlogPostAndCategoryNameByIdResponse 
            { 
                BlogPost = blogPost,
                CategoryName = categoryName 
            };
        }
    }
}
