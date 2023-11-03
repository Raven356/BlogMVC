using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.BLL.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public override string? ToString()
        {
            return Name;
        }
    }
}
