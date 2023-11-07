using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using BlogMVC.BLL.AuthorsOperations.GetAuthorById;
using BlogMVC.DAL.Models;
using BlogMVC.BLL.AuthorsOperations.CreateAuthor;
using BlogMVC.BLL.AuthorsOperations.AuthorsService;

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
            var author = await _authorsService.GetAuthorById(new GetAuthorByIdRequest { Id = id});

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
        public async Task<IActionResult> Create([Bind("Id,NickName")] Author author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                author.UserId = userId;
                await _authorsService.CreateAuthor(new CreateAuthorCommand { Author = author });
                return RedirectToAction("Create", "BlogPosts");
            }
            //ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", author.UserId);
            return View(author);
        }
    }
}
