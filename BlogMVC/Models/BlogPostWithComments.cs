namespace BlogMVC.Models
{
    public class BlogPostWithComments
    {
        public BlogPost BlogPostValue { get; set; }

        public List<Comment> CommentList { get; set; }

        public Comment NewComment { get; set; }
    }
}
