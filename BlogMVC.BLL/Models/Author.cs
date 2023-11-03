using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.BLL.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? NickName { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public override string? ToString()
        {
            return NickName;
        }
    }
}
