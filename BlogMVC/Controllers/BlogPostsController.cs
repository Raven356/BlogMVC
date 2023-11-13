using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.BlogPostService;
using AutoMapper;
using BlogMVC.BLL.Services.TagsService;
using System.Text;
using BlogMVC.Models;
using System.Security.Claims;

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
            List<BlogPostWithTagsDTO> blogPostWithTags = new List<BlogPostWithTagsDTO>();
            var blogs = await _blogPostService.GetAllBlogPosts(
            new BlogPostSearchParametersDTO
            {
                SearchAuthor = searchAuthor
            ,
                SearchCategory = searchCategory
            ,
                SearchTitle = searchTitle
            });

            if (!string.IsNullOrEmpty(tagName))
            {
                blogs = await _tagsService.GetBlogPostsByTag(tagName);
            }

            foreach (var blog in blogs)
            {
                blogPostWithTags
                    .Add(new BlogPostWithTagsDTO 
                    { 
                        BlogPost = blog,
                        Tags = _mapper.Map<IEnumerable<TagsDTO>>(await _tagsService.GetTagsByBlogPostId(blog.Id))
                    });
            }
            return View(blogPostWithTags);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var blogPostWithComments = await _blogPostService.GetBlogPostById(id);
            blogPostWithComments.Tags = 
                _mapper.Map<IEnumerable<TagsDTO>>
                (await _tagsService.GetTagsByBlogPostId(blogPostWithComments.BlogPostValue.Id));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blogPostWithCommentsViewModel = _mapper.Map<BlogPostWithCommentsViewModel>(blogPostWithComments);
            blogPostWithCommentsViewModel.IsAuthor = userId != null 
                && blogPostWithComments.BlogPostValue.Author!.UserId!.Equals(userId);

            return View(blogPostWithCommentsViewModel);
        }

        // GET: BlogPosts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var author = await _blogPostService.GetAuthorByUser(User);
            if (author == null)
            {
                return RedirectToAction("Create", "Authors");
            }
            return View(new CreateBlogPostDTO { AuthorId = author!.Id});
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBlogPostDTO blogPost)
        {
            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost.CategoryName);
                var tags = blogPost.Tags == null ? null 
                    : blogPost.Tags.Split(" ", StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
                var blog = await _blogPostService.CreateNewBlogPost(blogPost, categoryId);

                await _tagsService.CreateTags(tags, blog.Id);
                return RedirectToAction("Index", "BlogPosts");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await _blogPostService
                .GetBlogPostAndCategoryName(id);

            var editBlogPostViewModel = _mapper.Map<EditBlogPostViewModel>(response);

            var tags = await _tagsService.GetTagsByBlogPostId(id);
            var stringBuilder = new StringBuilder();
            foreach (var tag in tags)
            {
                stringBuilder.Append(tag.Name + " ");
            }
            editBlogPostViewModel.Tags = stringBuilder.ToString();
            
            return View(editBlogPostViewModel);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogPostViewModel blogPost)
        {
            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost.CategoryName);
                var editBlogPostDTO = _mapper.Map<EditBlogPostDTO>(blogPost);
                editBlogPostDTO.CategoryId = categoryId;

                await _blogPostService.EditBlogPost(editBlogPostDTO);
                await _tagsService.UpdateTags(blogPost.Tags == null ? null : blogPost.Tags
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()
                    , (int)blogPost.Id);
                return RedirectToAction("Details", new { id = blogPost.Id });
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

        private async Task<int> GetCategoryId(string categoryName)
        {
            return await _blogPostService
                .GetCategoryId(categoryName);
        }
    }
}
