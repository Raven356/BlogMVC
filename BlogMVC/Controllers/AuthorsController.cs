using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BlogMVC.BLL.Context;
using System.Drawing.Text;
using MediatR;
using BlogMVC.BLL.AuthorsOperations.GetAuthorById;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.AuthorsOperations.CreateAuthor;

namespace BlogMVC.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var author = await _mediator.Send(new GetAuthorByIdRequest { Id = id });

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
                await _mediator.Send(new CreateAuthorCommand { Author = author });
                return RedirectToAction("Create", "BlogPosts");
            }
            //ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", author.UserId);
            return View(author);
        }
    }
}
