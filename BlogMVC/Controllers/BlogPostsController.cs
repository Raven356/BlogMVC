using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogMVC.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly BlogMVCContext _context;
        private readonly UserManager<User> _userManager;

        public BlogPostsController(BlogMVCContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(string? searchTitle, string? searchCategory, string? searchAuthor)
        {
            var blogs = await _context.BlogPost.ToListAsync();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(searchTitle)).ToList();
            }

            if (!string.IsNullOrEmpty(searchCategory))
            {
                blogs = blogs.Where(b => _context.Category.Find(b.CategoryId).Name.Contains(searchCategory)).ToList();
            }

            if (!string.IsNullOrEmpty(searchAuthor))
            {
                blogs = blogs.Where(b => _context.Author.Find(b.AuthorId).NickName.Contains(searchAuthor)).ToList();
            }

            blogs.ForEach(b => b.Author = _context.Author.Find(b.AuthorId));
            blogs.ForEach(b => b.Category = _context.Category.Find(b.CategoryId));

            blogs = blogs.OrderBy(b => b.Title).ToList();
            return View(blogs);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogPost == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.Author = await _context.Author.FindAsync(blogPost.AuthorId);
            blogPost.Category = await _context.Category.FindAsync(blogPost.CategoryId);

            var comments = await _context.Comment.Where(c => c.BlogPostId == id).ToListAsync();
            comments.ForEach(c => c.User = _context.User.Find(c.UserId));
            BlogPostWithComments blogPostWithComments = 
                new BlogPostWithComments 
                { 
                    BlogPostValue = blogPost,
                    CommentList = comments,
                    NewComment = new Comment()
                };

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
                    await _context.AddAsync(blogPostWithComments.NewComment);
                    await _context.SaveChangesAsync();
                    blogPostWithComments.CommentList = await _context.Comment
                        .Where(c => c.BlogPostId == blogPostWithComments.BlogPostValue.Id).ToListAsync();
                    blogPostWithComments.CommentList.ForEach(c => c.User = _context.User.Find(c.UserId));
                    return RedirectToAction(nameof(Details), new { id = blogPostWithComments.BlogPostValue.Id });
                }

            }
            blogPostWithComments.CommentList = await _context.Comment
                        .Where(c => c.BlogPostId == blogPostWithComments.BlogPostValue.Id).ToListAsync();
            blogPostWithComments.CommentList.ForEach(c => c.User = _context.User.Find(c.UserId));
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
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;
            var author = await _context.Author.Where(a => a.UserId.Equals(userId)).FirstOrDefaultAsync();
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
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    int categoryId = await GetCategoryId(blogPost);
                   
                    await _context.AddAsync(new BlogPost { AuthorId = blogPost.AuthorId
                        , CategoryId = categoryId, Date = blogPost.Date
                        , Text = blogPost.Text, Title = blogPost.Title});
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            var result = await GetAuthorUserId();
            return result;
        }

        private async Task<int> GetCategoryId(BlogPostCreateViewModel blogPost)
        {
            int categoryId = -1;
            while (categoryId == -1)
            {
                var category = _context.Category.Where(c => c.Name == blogPost.CategoryName).FirstOrDefault();

                if (category == null)
                {
                    await _context.AddAsync(new Category { Name = blogPost.CategoryName });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    categoryId = category.Id;
                }
            }
            return categoryId;
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogPost == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            var authorId = await GetAuthorId();
            ViewData["AuthorId"] = authorId;
            var categoryName = (await _context.Category.FindAsync(blogPost.CategoryId)).Name;
            ViewData["CategoryName"] = categoryName;
            return View(new BlogPostCreateViewModel { Id = blogPost.Id, CategoryName = categoryName
                , AuthorId = authorId, Date = blogPost.Date
                , Text = blogPost.Text, Title = blogPost.Title});
        }

        private async Task<int> GetAuthorId()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;
            var author = await _context.Author.Where(a => a.UserId.Equals(userId)).FirstOrDefaultAsync();
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
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    int categoryId = await GetCategoryId(blogPost);
                    var blog = await _context.BlogPost.FindAsync(blogPost.Id);
                    blog.AuthorId = blogPost.AuthorId;
                    blog.CategoryId = categoryId;
                    blog.Text = blogPost.Text;
                    blog.Date = blogPost.Date;
                    blog.Title = blogPost.Title;
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
                return RedirectToAction(nameof(Details), new {id = blogPost.Id});
            }
            ViewData["AuthorId"] = await GetAuthorId();
            ViewData["CategoryId"] = blogPost.CategoryName;
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogPost == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BlogPost == null)
            {
                return Problem("Entity set 'BlogMVCContext.BlogPost'  is null.");
            }
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost != null)
            {
                _context.BlogPost.Remove(blogPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int? id)
        {
          return (_context.BlogPost?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
