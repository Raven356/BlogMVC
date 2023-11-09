using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.BlogPostService;
using AutoMapper;
using BlogMVC.BLL.Services.TagsService;
using System.Reflection.Metadata;
using System.Text;

namespace BlogMVC.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IMapper _mapper;
        private readonly ITagsService _tagsService;

        public BlogPostsController(IBlogPostService blogPostService, IMapper mapper, ITagsService tagsService)
        {
            _blogPostService = blogPostService;
            _mapper = mapper;
            _tagsService = tagsService;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(string? searchTitle, string? searchCategory
            , string? searchAuthor, string? tagName)
        {
            var blogs = await _blogPostService.GetAllBlogPosts(
                new GetBlogPostsDTO { SearchAuthor = searchAuthor
                , SearchCategory = searchCategory
                , SearchTitle = searchTitle, TagName = tagName });
            List<BlogPostWithTagsDTO> blogPostWithTags = new List<BlogPostWithTagsDTO>();
            foreach (var blog in blogs)
            {
                blogPostWithTags
                    .Add(new BlogPostWithTagsDTO 
                    { 
                        BlogPost = blog, Tags = await _tagsService.GetTagsByBlogPostId(blog.Id)
                    });
            }
            return View(blogPostWithTags);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var blogPostWithComments = await _blogPostService.GetBlogPostById(id);
            blogPostWithComments.Tags = await _tagsService.GetTagsByBlogPostId(blogPostWithComments.BlogPostValue.Id);
            return View(blogPostWithComments);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BlogPostWithCommentsDTO blogPostWithComments)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(blogPostWithComments.NewComment.Text))
                {
                    blogPostWithComments = await _blogPostService.AddNewComment(blogPostWithComments);
                    return RedirectToAction(nameof(Details), new { id = blogPostWithComments.BlogPostValue.Id });
                }

            }
            return View(blogPostWithComments);
        }

        // GET: BlogPosts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var result = await GetAuthorUserId();
            return result;
        }

        private async Task<IActionResult> GetAuthorUserId()
        {
            var author = await _blogPostService.GetAuthorByUserId(User);
            if (author == null)
            {
                return RedirectToAction("Create", "Authors");
            }
            ViewData["AuthorId"] = author.Id;
            return View();
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,CategoryName,Tags,Date,AuthorId")] BlogPostCreateDTO blogPost)
        {
            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost);
                var tags = blogPost.Tags.Split(" ", StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
                var blog = await _blogPostService.CreateNewBlogPost
                    (new CreateBlogPostDTO { CategoryId = categoryId, BlogPostCreateViewModel = blogPost });
                await _tagsService.CreateTags(tags, blog.Id);
                return RedirectToAction(nameof(Index));
            }
            var result = await GetAuthorUserId();
            return result;
        }

        private async Task<int> GetCategoryId(BlogPostCreateDTO blogPost)
        {
            return await _blogPostService
                .GetCategoryById(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await _blogPostService
                .GetBlogPostAndCategoryName(id);
            var blogPost = response.BlogPost;

            var authorId = await GetAuthorId();
            ViewData["AuthorId"] = authorId;
            var categoryName = response.CategoryName;
            ViewData["CategoryName"] = categoryName;
            var blogPostCreateDto = _mapper.Map<BlogPostCreateDTO>(blogPost);
            blogPostCreateDto.CategoryName = categoryName;
            blogPostCreateDto.AuthorId = authorId;
            var tags = await _tagsService.GetTagsByBlogPostId(blogPostCreateDto.Id);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var tag in tags)
            {
                stringBuilder.Append(tag.Name + " ");
            }
            blogPostCreateDto.Tags = stringBuilder.ToString();
            return View(blogPostCreateDto);
        }

        private async Task<int> GetAuthorId()
        {
            var author = await _blogPostService.GetAuthorByUserId(User);
            return author.Id;
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id
            , [Bind("Id,Title,Text,CategoryName,Date,AuthorId,Tags")] BlogPostCreateDTO blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost);
                await _blogPostService
                    .EditBlogPost(new EditBlogPostDTO { CategoryId = categoryId, CreateViewModel = blogPost });
                await _tagsService.UpdateTags(blogPost.Tags.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()
                    , (int)blogPost.Id);
                return RedirectToAction(nameof(Details), new {id = blogPost.Id});
            }
            ViewData["AuthorId"] = await GetAuthorId();
            ViewData["CategoryId"] = blogPost.CategoryName;
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _blogPostService.SimpleGetBlogPostById(id));
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogPostService.DeleteBlogPost(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
