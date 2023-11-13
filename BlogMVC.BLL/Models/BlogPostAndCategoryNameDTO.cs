using System.ComponentModel.DataAnnotations;

namespace BlogMVC.BLL.Models
{
    public class BlogPostAndCategoryNameDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }

        public string CategoryName { get; set; } = null!;
    }
}
