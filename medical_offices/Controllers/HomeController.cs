using medical_offices.Models;
using medical_offices.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace medical_offices.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
    }
}