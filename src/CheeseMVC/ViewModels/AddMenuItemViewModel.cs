using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public int CheeseID { get; set; }
        public int MenuID { get; set; }

        public Menu Menu { get; set; }
        public List<SelectList> Cheeses { get; set; }
    }
}
