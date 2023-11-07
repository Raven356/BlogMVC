using MediatR;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById
{
    public class GetBlogPostAndCategoryNameByIdRequest : IRequest<GetBlogPostAndCategoryNameByIdResponse>
    {
        public int? Id { get; set; }
    }
}
