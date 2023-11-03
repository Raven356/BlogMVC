using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.BLL.Models
{
    public class BlogPostCreateViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }
    }
}
