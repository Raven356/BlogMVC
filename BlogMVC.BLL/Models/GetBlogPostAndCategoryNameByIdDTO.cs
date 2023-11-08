using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.Models
{
    public class GetBlogPostAndCategoryNameByIdDTO
    {
        public BlogPost BlogPost { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
    }
}
