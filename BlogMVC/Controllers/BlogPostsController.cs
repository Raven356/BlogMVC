using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.BlogPostService;
using AutoMapper;
using BlogMVC.BLL.Services.TagsService;
using System.Text;
using BlogMVC.Models;
using Newtonsoft.Json;
using BlogMVC.DAL.Models;

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
            , string? searchAuthor)
        {
            List<BlogPostWithTagsDTO> blogPostWithTags = new List<BlogPostWithTagsDTO>();
            IEnumerable<BlogPostDTO>? blogs = TempData["BlogPosts"] == null ? null :
                JsonConvert.DeserializeObject<IEnumerable<BlogPostDTO>>(TempData["BlogPosts"].ToString());
            if (blogs == null || !blogs.Any())
            {
                blogs = await _blogPostService.GetAllBlogPosts(
                new GetBlogPostsDTO
                {
                    SearchAuthor = searchAuthor
                ,
                    SearchCategory = searchCategory
                ,
                    SearchTitle = searchTitle
                });
            }
            
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

        // GET: BlogPosts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var result = await CheckIfUserIsAuthor();
            return result;
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
                TempData["Tags"] = tags;
                return RedirectToAction("Create", "Tags", new { blogPostId = blog.Id });
            }
            var result = await CheckIfUserIsAuthor();
            return result;
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await _blogPostService
                .GetBlogPostAndCategoryName(id);
            var blogPost = response.BlogPost;

            var authorId = await GetAuthorId();
            var categoryName = response.CategoryName;
            var blogPostCreateDto = _mapper.Map<BlogPostCreateDTO>(blogPost);
            blogPostCreateDto.CategoryName = categoryName;
            blogPostCreateDto.AuthorId = authorId;
            var tags = JsonConvert.DeserializeObject<IEnumerable<Tags>>(TempData["TagsById"].ToString());
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var tag in tags)
            {
                stringBuilder.Append(tag.Name + " ");
            }
            blogPostCreateDto.Tags = stringBuilder.ToString();
            EditBlogPostDTO editBlogPostDTO = 
                new EditBlogPostDTO { CreateViewModel = blogPostCreateDto, CategoryId = blogPost.CategoryId };
            return View(editBlogPostDTO);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id
            , [Bind("CreateViewModel,CategoryId")] EditBlogPostDTO blogPost)
        {
            if (id != blogPost.CreateViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost.CreateViewModel);
                blogPost.CreateViewModel.AuthorId = await GetAuthorId();
                blogPost.CategoryId = categoryId;
                await _blogPostService.EditBlogPost(blogPost);

                TempData["UpdateTags"] = blogPost.CreateViewModel.Tags.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                return RedirectToAction("Update", "Tags", new { blogPostId = blogPost.CreateViewModel.Id});
            }
            
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

        private async Task<IActionResult> CheckIfUserIsAuthor()
        {
            var author = await _blogPostService.GetAuthorByUserId(User);
            if (author == null)
            {
                return RedirectToAction("Create", "Authors");
            }
            return View(new BlogPostCreateDTO { AuthorId = author.Id});
        }

        private async Task<int> GetAuthorId()
        {
            var author = await _blogPostService.GetAuthorByUserId(User);
            return author.Id;
        }

        private async Task<int> GetCategoryId(BlogPostCreateDTO blogPost)
        {
            return await _blogPostService
                .GetCategoryById(blogPost);
        }
    }
}
