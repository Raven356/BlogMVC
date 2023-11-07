using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostsById
{
    public class GetBlogPostsByIdRequestHandler : IRequestHandler<GetBlogPostsByIdRequest, BlogPostWithComments>
    {
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Author> _authorRepository;

        public GetBlogPostsByIdRequestHandler(IRepository<Category> categoryRepository
            , IRepository<Comment> commentRepository
            , IRepository<User> userRepository
            , IRepository<BlogPost> blogPostRepository
            , IRepository<Author> authorRepository)
        {
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _blogPostRepository = blogPostRepository;
            _authorRepository = authorRepository;
        }

        public async Task<BlogPostWithComments> Handle(GetBlogPostsByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _blogPostRepository.GetAll().AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            blogPost.Author = await _authorRepository.GetById(blogPost.AuthorId);
            blogPost.Category = await _categoryRepository.GetById(blogPost.CategoryId);

            var comments = await _commentRepository.GetAll().AsQueryable().Where(c => c.BlogPostId == request.Id).ToListAsync();
            comments.ForEach(c => c.User = _userRepository.GetById(c.UserId).Result);
            BlogPostWithComments blogPostWithComments =
                new BlogPostWithComments
                {
                    BlogPostValue = blogPost,
                    CommentList = comments,
                    NewComment = new Comment()
                };
            return blogPostWithComments;
        }
    }
}
