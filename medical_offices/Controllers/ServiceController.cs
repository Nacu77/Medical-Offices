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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            List<Service> services = ctx.Services.ToList();
            ViewBag.Services = services;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Service service = ctx.Services.Find(id);
                if(service != null)
                {
                    return View(service);
                }
                return HttpNotFound("Couldn't find the service with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing service id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            Service service = new Service();
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(Service serviceRequest)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    ctx.Services.Add(serviceRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(serviceRequest);
            }
            catch(Exception)
            {
                return View(serviceRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Service service = ctx.Services.Find(id);
                if(service == null)
                {
                    return HttpNotFound("Couldn't find the service with id " + id.ToString() + "!");
                }
                return View(service);
            }
            return HttpNotFound("Missing service id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Service serviceRequest)
        {
            Service service = ctx.Services.Find(id);
            try
            {
                if(ModelState.IsValid)
                {
                    if(TryUpdateModel(service))
                    {
                        service.Name = serviceRequest.Name;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(serviceRequest);
            }
            catch(Exception)
            {
                return View(serviceRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Service service = ctx.Services.Find(id);
            if(service != null)
            {
                ctx.Services.Remove(service);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the service with id " + id.ToString() + "!");
        }
    }
}