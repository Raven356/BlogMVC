using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.CategoriesOperations
{
    public class GetCategoriesByIdRequestHandler : IRequestHandler<GetCategoriesByIdRequest, Category>
    {
        private readonly IRepository<Category> _categoryRepository;

        public GetCategoriesByIdRequestHandler(IRepository<Category> repository)
        {
            _categoryRepository = repository;
        }

        public async Task<Category> Handle(GetCategoriesByIdRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAll().AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            return category;
        }
    }
}
