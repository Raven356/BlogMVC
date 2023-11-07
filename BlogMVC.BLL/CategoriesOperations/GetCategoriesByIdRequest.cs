using BlogMVC.DAL.Models;
using MediatR;

namespace BlogMVC.BLL.CategoriesOperations
{
    public class GetCategoriesByIdRequest : IRequest<Category>
    {
        public int? Id { get; set; }
    }
}
