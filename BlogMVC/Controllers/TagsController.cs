using BlogMVC.BLL.Services.TagsService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogMVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        public async Task<IActionResult> Index(string? tagName)
        {
            var blogs = await _tagsService.GetBlogPostsByTag(tagName);
            TempData["BlogPosts"] = JsonConvert.SerializeObject(blogs);
            return RedirectToAction("Index", "BlogPosts");
        }

        public async Task<IActionResult> Create(int? blogPostId)
        {
            var tags = TempData["Tags"] as IEnumerable<string>;
            await _tagsService.CreateTags(tags!, (int)blogPostId!);
            return RedirectToAction("Index", "BlogPosts");
        }

        public async Task<IActionResult> Update(int? blogPostId)
        {
            await _tagsService.UpdateTags(TempData["UpdateTags"] as IEnumerable<string>, (int)blogPostId!);
            return RedirectToAction("Details", "BlogPosts", new { id = blogPostId });
        }

        public async Task<IActionResult> GetByBlogId(int? id)
        {
            var tags = await _tagsService.GetTagsByBlogPostId(id);
            TempData["TagsById"] = JsonConvert.SerializeObject(tags);
            return RedirectToAction("Edit", "BlogPosts", new { id = id });
        }
    }
}
