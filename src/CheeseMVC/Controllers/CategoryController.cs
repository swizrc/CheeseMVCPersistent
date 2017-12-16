﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CheeseDbContext context;

        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<CheeseCategory> Categories = context.Categories.ToList();
            return View(Categories);
        }

        public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                CheeseCategory cheeseCategory = new CheeseCategory
                {
                    Name = addCategoryViewModel.Name
                };

                context.Categories.Add(cheeseCategory);
                context.SaveChanges();
                return Redirect("/Category");
            }

            return View(addCategoryViewModel);
        }
    }
}