using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.ControllersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostWithCommentsDTO blogPostWithComments)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(blogPostWithComments.NewComment.Text))
                {
                    blogPostWithComments = await _commentsService.AddNewComment(blogPostWithComments);
                    return RedirectToAction("Details", "BlogPosts", new { id = blogPostWithComments.BlogPostValue.Id });
                }

            }
            return RedirectToAction("Details", "BlogPosts");
        }
    }
}
