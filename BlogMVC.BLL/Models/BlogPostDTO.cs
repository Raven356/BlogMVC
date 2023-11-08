using BlogMVC.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models
{
    public class BlogPostDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        public Category? Category { get; set; }
    }
}
