﻿using BlogMVC.Models;

namespace BlogMVC.BLL.Models
{
    public class BlogPostWithCommentsDTO
    {
        public BlogPostDTO BlogPostValue { get; set; } = null!;

        public IEnumerable<CommentDTO> CommentList { get; set; } = null!;

        public CommentDTO NewComment { get; set; } = null!;

        public IEnumerable<TagsDTO> Tags { get; set; } = null!;
    }
}
