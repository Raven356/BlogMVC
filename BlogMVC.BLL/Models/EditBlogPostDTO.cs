namespace BlogMVC.BLL.Models
{
    public class EditBlogPostDTO
    {
        public BlogPostCreateDTO CreateViewModel { get; set; } = null!;

        public int CategoryId { get; set; }
    }
}
