using AutoMapper;
using BlogMVC.BLL.BlogPostOperations.AddNewComment;
using BlogMVC.BLL.BlogPostOperations.CreateBlogPost;
using BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById;
using BlogMVC.BLL.BlogPostOperations.EditBlogPost;
using BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts;
using BlogMVC.BLL.BlogPostOperations.GetAuthorIdByUser;
using BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById;
using BlogMVC.BLL.BlogPostOperations.GetBlogPostsById;
using BlogMVC.BLL.BlogPostOperations.GetCategoryById;
using BlogMVC.BLL.BlogPostOperations.SimpleGetById;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.BlogPostOperations.BlogPostService
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public BlogPostService(IRepository<Category> categoryRepository
            , IRepository<Comment> commentRepository
            , IRepository<User> userRepository
            , IRepository<BlogPost> blogPostRepository
            , IRepository<Author> authorRepository
            , IMapper mapper
            , UserManager<User> userManager)
        {
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _blogPostRepository = blogPostRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BlogPostWithComments> AddNewComment(AddNewCommentCommand request)
        {
            await _commentRepository.Add(request.BlogPostWithComments.NewComment);

            request.BlogPostWithComments.CommentList = _commentRepository.GetAll()
                .Where(c => c.BlogPostId == request.BlogPostWithComments.BlogPostValue.Id).ToList();

            request.BlogPostWithComments.CommentList.ToList()
                .ForEach(c => c.User = _userRepository.GetById(c.UserId).Result);

            return request.BlogPostWithComments;
        }

        public async Task CreateNewBlogPost(CreateBlogPostCommand request)
        {
            await _blogPostRepository.Add(_mapper.Map<BlogPost>(request));
            return;
        }

        public async Task DeleteBlogPost(DeleteBlogPostByIdCommand request)
        {
            await _blogPostRepository.Delete(request.Id);
            return;
        }

        public async Task EditBlogPost(EditBlogPostCommand request)
        {
            var blog = await _blogPostRepository.GetById(request.CreateViewModel.Id);
            _mapper.Map(request, blog);
            await _blogPostRepository.Update(blog);
            return;
        }

        public async Task<List<BlogPost>> GetAllBlogPosts(GetBlogPostsRequest request)
        {
            var blogs = _blogPostRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrEmpty(request.SearchCategory))
            {
                var category = _categoryRepository.GetAll().AsQueryable();
                blogs = blogs.Where(b => category.Where(c => c.Id == b.CategoryId).First()
                    .Name.Contains(request.SearchCategory));
            }

            if (!string.IsNullOrEmpty(request.SearchAuthor))
            {
                var author = _authorRepository.GetAll().AsQueryable();
                blogs = blogs.Where(b => author.Where(a => a.Id == b.AuthorId).First()
                    .NickName.Contains(request.SearchAuthor));
            }

            await blogs.ForEachAsync(b => b.Author = _authorRepository.GetById(b.AuthorId).Result);
            await blogs.ForEachAsync(b => b.Category = _categoryRepository.GetById(b.CategoryId).Result);

            return await blogs.OrderBy(b => b.Title).ToListAsync();
        }

        public async Task<Author> GetAuthorByUserId(GetUserAuthorByUserId request)
        {
            var user = await _userManager.GetUserAsync(request.User);
            string userId = user.Id;
            var author = await _authorRepository.GetAll().AsQueryable()
                .Where(a => a.UserId.Equals(userId)).FirstOrDefaultAsync();
            return author;
        }

        public async Task<GetBlogPostAndCategoryNameByIdResponse> 
            GetBlogPostAndCategoryName(GetBlogPostAndCategoryNameByIdRequest request)
        {
            var blogPost = await _blogPostRepository.GetById(request.Id);

            var categoryName = (await _categoryRepository.GetById(blogPost.CategoryId)).Name;

            return new GetBlogPostAndCategoryNameByIdResponse
            {
                BlogPost = blogPost,
                CategoryName = categoryName
            };
        }

        public async Task<BlogPostWithComments> GetBlogPostById(GetBlogPostsByIdRequest request)
        {
            var blogPost = await _blogPostRepository.GetAll().AsQueryable()
               .FirstOrDefaultAsync(m => m.Id == request.Id);

            blogPost.Author = await _authorRepository.GetById(blogPost.AuthorId);
            blogPost.Category = await _categoryRepository.GetById(blogPost.CategoryId);

            var comments = await _commentRepository.GetAll().AsQueryable()
                .Where(c => c.BlogPostId == request.Id).ToListAsync();

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

        public async Task<int> GetCategoryById(GetCategoryByIdRequest request)
        {
            int categoryId = -1;
            while (categoryId == -1)
            {
                var category = _categoryRepository.GetAll().AsQueryable()
                    .Where(c => c.Name == request.BlogPostCreateViewModel.CategoryName)
                    .FirstOrDefault();

                if (category == null)
                {
                    await _categoryRepository.Add(new Category { Name = request.BlogPostCreateViewModel.CategoryName });
                }
                else
                {
                    categoryId = category.Id;
                }
            }
            return categoryId;
        }

        public async Task<BlogPost> SimpleGetBlogPostById(SimpleGetByIdRequest request)
        {
            var blogPost = await _blogPostRepository.GetAll().AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            return blogPost;
        }
    }
}
