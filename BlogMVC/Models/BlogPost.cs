﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.Models
{
    public class BlogPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

    }
}
