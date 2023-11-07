﻿using Microsoft.AspNetCore.Mvc;
using BlogMVC.BLL.CategoriesOperations;
using BlogMVC.BLL.CategoriesOperations.CategoriesService;

namespace BlogMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var category = await _categoriesService.GetCategoryById(new GetCategoriesByIdRequest { Id = id });
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}
