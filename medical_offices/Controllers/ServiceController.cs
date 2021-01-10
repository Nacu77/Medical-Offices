using medical_offices.Models;
using medical_offices.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace medical_offices.Controllers
{
    public class ServiceController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            List<Service> services = ctx.Services.ToList();
            ViewBag.Services = services;
            return View();
        }
    }
}