using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogMVC.BLL.Services.AuthorsService;
using BlogMVC.Models;

namespace BlogMVC.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var author = await _authorsService.GetAuthorById(id);

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NickName, UserId")] AuthorDTO author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                author.UserId = userId;
                await _authorsService.CreateAuthor(author);
                return RedirectToAction("Create", "BlogPosts");
            }
            return View(author);
        }
    }
}
