using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogMVC.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly BlogMVCContext _context;
        private readonly UserManager<User> _userManager;

        public AuthorsController(BlogMVCContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            author.User = await _context.User.FindAsync(author.UserId);

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
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "BlogPosts");
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", author.UserId);
            return View(author);
        }
    }
}
