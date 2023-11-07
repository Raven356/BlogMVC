using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.GetCategoryById
{
    public class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, int>
    {
        private readonly IRepository<Category> _repository;

        public GetCategoryByIdRequestHandler(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
        {
            int categoryId = -1;
            while (categoryId == -1)
            {
                var category = _repository.GetAll().AsQueryable()
                    .Where(c => c.Name == request.BlogPostCreateViewModel.CategoryName)
                    .FirstOrDefault();

                if (category == null)
                {
                    await _repository.Add(new Category { Name = request.BlogPostCreateViewModel.CategoryName });
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
