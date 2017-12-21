using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();

            return View(menus);
        }

        public IActionResult ViewMenu(int id)
        {
                List<CheeseMenu> items = context
            .CheeseMenus
            .Include(item => item.Cheese)
            .Where(cm => cm.MenuID == id)
            .ToList();

            Menu theMenu = context.Menus.Single(c => c.ID == id);

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel { Menu = theMenu, Items = items };

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu theMenu = context.Menus.Single(c => c.ID == id);

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel (context.Cheeses.ToList(),theMenu);

            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {

            if (ModelState.IsValid)
            {
                var cheeseID = addMenuItemViewModel.cheeseID;
                var menuID = addMenuItemViewModel.menuID;

                IList<CheeseMenu> existingItems = context.CheeseMenus
                .Where(cm => cm.CheeseID == addMenuItemViewModel.cheeseID)
                .Where(cm => cm.MenuID == addMenuItemViewModel.menuID).ToList();

                if(existingItems.Count == 0)
                {
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        Cheese = context.Cheeses.Single(c => c.ID == cheeseID),
                        Menu = context.Menus.Single(m => m.ID == menuID)
                    };
                    context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();
                }
                return Redirect("/Menu/ViewMenu/" + menuID.ToString());
            }

            IEnumerable<Cheese> cheeses = context.Cheeses.ToList();

            addMenuItemViewModel.Cheeses = new List<SelectListItem>();

            foreach (var cheese in cheeses)
            {
                addMenuItemViewModel.Cheeses.Add(new SelectListItem { Value = cheese.ID.ToString(), Text = cheese.Name });
            }

            return View(addMenuItemViewModel);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();

            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu { Name = addMenuViewModel.Name };

                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }
    }
}