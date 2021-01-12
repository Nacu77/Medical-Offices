using medical_offices.Models;
using medical_offices.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace medical_offices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.People = ctx.People.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Person person = ctx.People.Find(id);
                if (person != null)
                {
                    return View(person);
                }
                return HttpNotFound("Couldn't find the person with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing person id parameter!");
        }

        [HttpGet]
        public ActionResult New()
        {
            Person person = new Person();
            return View(person);
        }

        [HttpPost]
        public ActionResult New(Person personRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ctx.People.Add(personRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(personRequest);
            }
            catch (Exception)
            {
                return View(personRequest);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Person person = ctx.People.Find(id);
                if (person == null)
                {
                    return HttpNotFound("Couldn't find the person with id " + id.ToString() + "!");
                }
                return View(person);
            }
            return HttpNotFound("Missing person id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Person personRequest)
        {
            Person person = ctx.People.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(person))
                    {
                        person.FirstName = personRequest.FirstName;
                        person.LastName = personRequest.LastName;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(personRequest);
            }
            catch (Exception)
            {
                return View(personRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Person person = ctx.People.Find(id);
            if (person != null)
            {
                ctx.People.Remove(person);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the person with id " + id.ToString() + "!");
        }
    }
}