using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BlogMVC.BLL.Context;
using MediatR;
using BlogMVC.BLL.BlogPostOperations;
using BlogMVC.BLL.BlogPostOperations.GetBlogPostsById;
using BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.BlogPostOperations.AddNewComment;
using BlogMVC.BLL.BlogPostOperations.GetAuthorIdByUser;
using BlogMVC.BLL.BlogPostOperations.CreateBlogPost;
using BlogMVC.BLL.BlogPostOperations.GetCategoryById;
using BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById;
using BlogMVC.BLL.BlogPostOperations.EditBlogPost;
using BlogMVC.BLL.BlogPostOperations.SimpleGetById;
using BlogMVC.BLL.BlogPostOperations.DeleteBlogPostById;

namespace BlogMVC.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly IMediator _mediator;

        public BlogPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(string? searchTitle, string? searchCategory, string? searchAuthor)
        {
            var blogs = await _mediator.Send(new GetBlogPostsRequest { SearchAuthor = searchAuthor, SearchCategory = searchCategory, SearchTitle = searchTitle });
            return View(blogs);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var blogPostWithComments = await _mediator.Send(new GetBlogPostsByIdRequest { Id = id });
            return View(blogPostWithComments);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BlogPostWithComments blogPostWithComments)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(blogPostWithComments.NewComment.Text))
                {
                    blogPostWithComments = await _mediator.Send(new AddNewCommentCommand { BlogPostWithComments =  blogPostWithComments });
                    return RedirectToAction(nameof(Details), new { id = blogPostWithComments.BlogPostValue.Id });
                }

            }
            //blogPostWithComments.CommentList = await _context.Comment
            //            .Where(c => c.BlogPostId == blogPostWithComments.BlogPostValue.Id).ToListAsync();
            //blogPostWithComments.CommentList.ForEach(c => c.User = _context.User.Find(c.UserId));
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
            var author = await _mediator.Send(new GetUserAuthorByUserId { User = User });
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
        public async Task<IActionResult> Create([Bind("Id,Title,Text,CategoryName,Date,AuthorId")] BlogPostCreateViewModel blogPost)
        {
            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost);
                await _mediator.Send(new CreateBlogPostCommand { CategoryId = categoryId, BlogPostCreateViewModel = blogPost });
                return RedirectToAction(nameof(Index));
            }
            var result = await GetAuthorUserId();
            return result;
        }

        private async Task<int> GetCategoryId(BlogPostCreateViewModel blogPost)
        {
            return await _mediator.Send(new GetCategoryByIdRequest { BlogPostCreateViewModel = blogPost});
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await _mediator.Send(new GetBlogPostAndCategoryNameByIdRequest { Id = id });
            var blogPost = response.BlogPost;

            var authorId = await GetAuthorId();
            ViewData["AuthorId"] = authorId;
            var categoryName = response.CategoryName;
            ViewData["CategoryName"] = categoryName;
            return View(new BlogPostCreateViewModel { Id = blogPost.Id, CategoryName = categoryName
                , AuthorId = authorId, Date = blogPost.Date
                , Text = blogPost.Text, Title = blogPost.Title});
        }

        private async Task<int> GetAuthorId()
        {
            var author = await _mediator.Send(new GetUserAuthorByUserId { User = User });
            return author.Id;
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,CategoryName,Date,AuthorId")] BlogPostCreateViewModel blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int categoryId = await GetCategoryId(blogPost);
                await _mediator.Send(new EditBlogPostCommand { CategoryId = categoryId, CreateViewModel = blogPost });
                return RedirectToAction(nameof(Details), new {id = blogPost.Id});
            }
            ViewData["AuthorId"] = await GetAuthorId();
            ViewData["CategoryId"] = blogPost.CategoryName;
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _mediator.Send(new SimpleGetByIdRequest { Id = id}));
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteBlogPostByIdCommand { Id = id});
            return RedirectToAction(nameof(Index));
        }
    }
}
