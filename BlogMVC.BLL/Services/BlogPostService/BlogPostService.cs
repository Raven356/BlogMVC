using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogMVC.BLL.Services.BlogPostService
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

        public BlogPostService(
            IRepository<Category> categoryRepository,
            IRepository<Comment> commentRepository,
            IRepository<User> userRepository,
            IRepository<BlogPost> blogPostRepository,
            IRepository<Author> authorRepository,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _blogPostRepository = blogPostRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BlogPost> CreateNewBlogPost(CreateBlogPostDTO request, int categoryId)
        {
            var newBlog = _mapper.Map<BlogPost>(request);
            newBlog.CategoryId = categoryId;
            return await _blogPostRepository.Add(newBlog);
        }

        public async Task DeleteBlogPost(int request)
        {
            await _blogPostRepository.Delete(request);
            return;
        }

        public async Task EditBlogPost(EditBlogPostDTO request)
        {
            var blog = _mapper.Map<BlogPost>(request);
            await _blogPostRepository.Update(blog);
            return;
        }

        public async Task<IEnumerable<BlogPostDTO>> GetAllBlogPosts(BlogPostSearchParametersDTO request)
        {
            var blogs = _blogPostRepository.GetAll()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrEmpty(request.SearchCategory))
            {
                var category = _categoryRepository.GetAll();
                blogs = blogs.Where(b => category.Where(c => c.Id == b.CategoryId).First()
                    .Name.Contains(request.SearchCategory));
            }

            if (!string.IsNullOrEmpty(request.SearchAuthor))
            {
                var author = _authorRepository.GetAll();
                blogs = blogs.Where(b => author.Where(a => a.Id == b.AuthorId).First()
                    .NickName!.Contains(request.SearchAuthor));
            }
            var result = _mapper.Map<IEnumerable<BlogPostDTO>>(blogs);
            return result;
        }

        public async Task<AuthorDTO> GetAuthorByUser(ClaimsPrincipal request)
        {
            var user = await _userManager.GetUserAsync(request);
            string userId = user.Id;
            var author = await _authorRepository.GetAll().
                FirstOrDefaultAsync(a => a.UserId!.Equals(userId));
            var result = _mapper.Map<AuthorDTO>(author);
            return result;
        }

        public async Task<BlogPostAndCategoryNameDTO>
            GetBlogPostAndCategoryName(int? id)
        {
            var blogPost = await _blogPostRepository.GetById(id);

            var categoryName = (await _categoryRepository.GetById(blogPost.CategoryId)).Name;

            var blogPostAndCategoryNameDTO = _mapper.Map<BlogPostAndCategoryNameDTO>(blogPost);
            blogPostAndCategoryNameDTO.CategoryName = categoryName;
            return blogPostAndCategoryNameDTO;
        }

        public async Task<BlogPostWithCommentsDTO> GetBlogPostById(int? id)
        {
            var blogPost = await _blogPostRepository.GetAll()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            var comments = await _commentRepository.GetAll()
                .Where(c => c.BlogPostId == id).ToListAsync();

            comments.ForEach(c => c.User = _userRepository.GetById(c.UserId!).Result);
            BlogPostWithCommentsDTO blogPostWithComments =
                new BlogPostWithCommentsDTO
                {
                    BlogPostValue = _mapper.Map<BlogPostDTO>(blogPost),
                    CommentList = _mapper.Map<IEnumerable<CommentDTO>>(comments),
                    NewComment = new CommentDTO()
                };
            return blogPostWithComments;
        }

        public async Task<int> GetCategoryId(string categoryName)
        {
            int categoryId = -1;
            while (categoryId == -1)
            {
                var category = _categoryRepository.GetAll()
                    .Where(c => c.Name == categoryName)
                    .FirstOrDefault();

                if (category == null)
                {
                    await _categoryRepository.Add(new Category { Name = categoryName });
                }
                else
                {
                    categoryId = category.Id;
                }
            }
            return categoryId;
        }

        public async Task<BlogPostDTO> SimpleGetBlogPostById(int? id)
        {
            var blogPost = await _blogPostRepository.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            var result = _mapper.Map<BlogPostDTO>(blogPost);
            return result;
        }
    }
}
