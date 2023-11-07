using BlogMVC.BLL.Models;
using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.GetCategoryById
{
    public class GetCategoryByIdRequest : IRequest<int>
    {
        public BlogPostCreateViewModel BlogPostCreateViewModel { get; set; }
    }
}
