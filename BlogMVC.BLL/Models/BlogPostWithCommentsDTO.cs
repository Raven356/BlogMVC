using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.Models
{
    public class BlogPostWithCommentsDTO
    {
        public BlogPost BlogPostValue { get; set; } = null!;

        public IEnumerable<Comment> CommentList { get; set; } = null!;

        public Comment NewComment { get; set; } = null!;
    }
}
