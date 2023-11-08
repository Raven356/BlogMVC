namespace BlogMVC.BLL.Models
{
    public class GetBlogPostsDTO
    {
        public string? SearchTitle { get; set; }

        public string? SearchCategory { get; set; }

        public string? SearchAuthor { get; set; }
    }
}
