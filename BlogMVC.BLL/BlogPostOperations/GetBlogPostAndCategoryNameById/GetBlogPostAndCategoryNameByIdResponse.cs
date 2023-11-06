using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById
{
    public class GetBlogPostAndCategoryNameByIdResponse
    {
        public BlogPost BlogPost { get; set; }

        public string CategoryName { get; set; }
    }
}
