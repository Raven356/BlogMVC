using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using MediatR;
using BlogMVC.BLL.CategoriesOperations;

namespace BlogMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var category = await _mediator.Send(new GetCategoriesByIdRequest { Id = id });
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}
