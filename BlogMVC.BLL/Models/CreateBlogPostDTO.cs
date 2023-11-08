namespace BlogMVC.BLL.Models
{
    public class CreateBlogPostDTO
    {
        public BlogPostCreateDTO BlogPostCreateViewModel { get; set; } = null!;

        public int CategoryId { get; set; }
    }
}
