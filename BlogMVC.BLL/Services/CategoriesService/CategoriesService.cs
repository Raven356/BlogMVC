using AutoMapper;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.CategoriesService
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> GetCategoryById(int? request)
        {
            var category = await _categoryRepository.GetAll()
                .FirstOrDefaultAsync(m => m.Id == request);
            var result = _mapper.Map<CategoryDTO>(category);
            return result;
        }
    }
}
