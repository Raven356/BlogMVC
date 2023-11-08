using BlogMVC.Models;

namespace BlogMVC.BLL.Services.CategoriesService
{
    public interface ICategoriesService
    {
        Task<CategoryDTO> GetCategoryById(int? request);
    }
}
