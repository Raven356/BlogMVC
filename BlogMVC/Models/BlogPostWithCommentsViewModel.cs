using BlogMVC.BLL.Models;

namespace BlogMVC.Models
{
    public class BlogPostWithCommentsViewModel
    {
        public BlogPostDTO BlogPostValue { get; set; } = null!;

        public IEnumerable<CommentDTO> CommentList { get; set; } = null!;

        public CommentDTO NewComment { get; set; } = null!;

        public IEnumerable<TagsDTO> Tags { get; set; } = null!;

        public bool IsAuthor { get; set; } = default;
    }
}
