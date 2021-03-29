using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPAProject.UI.Controllers
{
    public class SPAController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
