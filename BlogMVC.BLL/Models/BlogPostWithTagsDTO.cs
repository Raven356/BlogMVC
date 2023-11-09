using BlogMVC.DAL.Models;
using BlogMVC.Models;

namespace BlogMVC.BLL.Models
{
    public class BlogPostWithTagsDTO
    {
        public BlogPostDTO BlogPost { get; set; } = null!;

        public IEnumerable<Tags> Tags { get; set; } = Enumerable.Empty<Tags>();
    }
}
