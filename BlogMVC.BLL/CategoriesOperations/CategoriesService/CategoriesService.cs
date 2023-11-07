using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.CategoriesOperations.CategoriesService
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> GetCategoryById(GetCategoriesByIdRequest request)
        {
            var category = await _categoryRepository.GetAll().AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            return category;
        }
    }
}
