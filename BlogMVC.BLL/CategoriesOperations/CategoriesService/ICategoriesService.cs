using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.CategoriesOperations.CategoriesService
{
    public interface ICategoriesService
    {
        Task<Category> GetCategoryById(GetCategoriesByIdRequest request);
    }
}
