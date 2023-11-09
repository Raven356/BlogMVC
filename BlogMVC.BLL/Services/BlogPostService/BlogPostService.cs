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
        private readonly IRepository<Tags> _tagsRepository;
        private readonly IRepository<TagToBlogPost> _tagToBlogPostRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public BlogPostService(
            IRepository<Category> categoryRepository,
            IRepository<Comment> commentRepository,
            IRepository<User> userRepository,
            IRepository<BlogPost> blogPostRepository,
            IRepository<Author> authorRepository,
            IRepository<Tags> tagsRepository,
            IRepository<TagToBlogPost> tagToBlogPostRepository,
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
            _tagsRepository = tagsRepository;
            _tagToBlogPostRepository = tagToBlogPostRepository;
        }

        public async Task<BlogPostWithCommentsDTO> AddNewComment(BlogPostWithCommentsDTO request)
        {
            await _commentRepository.Add(request.NewComment);

            request.CommentList = _commentRepository.GetAll()
                .Where(c => c.BlogPostId == request.BlogPostValue.Id).ToList();

            request.CommentList.ToList()
                .ForEach(c => c.User = _userRepository.GetById(c.UserId!).Result);

            return request;
        }

        public async Task<BlogPost> CreateNewBlogPost(CreateBlogPostDTO request)
        {
            return await _blogPostRepository.Add(_mapper.Map<BlogPost>(request));
        }

        public async Task DeleteBlogPost(int request)
        {
            await _blogPostRepository.Delete(request);
            return;
        }

        public async Task EditBlogPost(EditBlogPostDTO request)
        {
            var blog = await _blogPostRepository.GetById(request.CreateViewModel.Id);
            _mapper.Map(request, blog);
            await _blogPostRepository.Update(blog);
            return;
        }

        public async Task<List<BlogPostDTO>> GetAllBlogPosts(GetBlogPostsDTO request)
        {
            var blogs = _blogPostRepository.GetAll();

            if (!string.IsNullOrEmpty(request.TagName))
            {
                var tag = await _tagsRepository.GetAll().Where(t => t.Name == request.TagName).FirstAsync();
                var tagToRepo = _tagToBlogPostRepository.GetAll().Where(t => t.TagId == tag.Id);
                blogs = blogs.Where(b => tagToRepo.Select(t => t.BlogPostId).Contains(b.Id));
            }

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

            await blogs.ForEachAsync(b => b.Author = _authorRepository.GetById(b.AuthorId).Result);
            await blogs.ForEachAsync(b => b.Category = _categoryRepository.GetById(b.CategoryId).Result);
            var result = new List<BlogPostDTO>();
            await blogs.OrderBy(b => b.Title).ForEachAsync(b => result.Add(_mapper.Map<BlogPostDTO>(b)));
            return result;
        }

        public async Task<AuthorDTO> GetAuthorByUserId(ClaimsPrincipal request)
        {
            var user = await _userManager.GetUserAsync(request);
            string userId = user.Id;
            var author = await _authorRepository.GetAll()
                .Where(a => a.UserId!.Equals(userId)).FirstOrDefaultAsync();
            var result = _mapper.Map<AuthorDTO>(author);
            return result;
        }

        public async Task<GetBlogPostAndCategoryNameByIdDTO>
            GetBlogPostAndCategoryName(int? request)
        {
            var blogPost = await _blogPostRepository.GetById(request);

            var categoryName = (await _categoryRepository.GetById(blogPost.CategoryId)).Name;

            return new GetBlogPostAndCategoryNameByIdDTO
            {
                BlogPost = blogPost,
                CategoryName = categoryName
            };
        }

        public async Task<BlogPostWithCommentsDTO> GetBlogPostById(int? request)
        {
            var blogPost = await _blogPostRepository.GetAll()
               .FirstOrDefaultAsync(m => m.Id == request);

            blogPost!.Author = await _authorRepository.GetById(blogPost.AuthorId);
            blogPost.Category = await _categoryRepository.GetById(blogPost.CategoryId);

            var comments = await _commentRepository.GetAll()
                .Where(c => c.BlogPostId == request).ToListAsync();

            comments.ForEach(c => c.User = _userRepository.GetById(c.UserId!).Result);
            BlogPostWithCommentsDTO blogPostWithComments =
                new BlogPostWithCommentsDTO
                {
                    BlogPostValue = blogPost,
                    CommentList = comments,
                    NewComment = new Comment()
                };
            return blogPostWithComments;
        }

        public async Task<int> GetCategoryById(BlogPostCreateDTO request)
        {
            int categoryId = -1;
            while (categoryId == -1)
            {
                var category = _categoryRepository.GetAll()
                    .Where(c => c.Name == request.CategoryName)
                    .FirstOrDefault();

                if (category == null)
                {
                    await _categoryRepository.Add(new Category { Name = request.CategoryName });
                }
                else
                {
                    categoryId = category.Id;
                }
            }
            return categoryId;
        }

        public async Task<BlogPostDTO> SimpleGetBlogPostById(int? request)
        {
            var blogPost = await _blogPostRepository.GetAll()
                .FirstOrDefaultAsync(m => m.Id == request);
            var result = _mapper.Map<BlogPostDTO>(blogPost);
            return result;
        }
    }
}
