using BlogMVC.Models;

namespace BlogMVC.BLL.Models
{
    public class BlogPostWithTagsDTO
    {
        public BlogPostDTO BlogPost { get; set; } = null!;

        public IEnumerable<TagsDTO> Tags { get; set; } = Enumerable.Empty<TagsDTO>();
    }
}
