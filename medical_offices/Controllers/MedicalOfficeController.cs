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
    public class MedicalOfficeController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MedicalOffices = ctx.MedicalOffices.ToList();
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id.HasValue)
            {
                MedicalOffice medicalOffice = ctx.MedicalOffices.Find(id);
                if(medicalOffice != null)
                {
                    return View(medicalOffice);
                }
                return HttpNotFound("Couldn't find the medical office with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing medical office id parameter!");
        }

        [Authorize]
        [HttpGet]
        public ActionResult New()
        {
            MedicalOffice medicalOffice = new MedicalOffice();
            medicalOffice.Services = new List<Service>();
            medicalOffice.ServicesList = GetAllServices();
            return View(medicalOffice);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(MedicalOffice medicalOfficeRequest)
        {
            var selectedServices = medicalOfficeRequest.ServicesList.Where(service => service.Checked).ToList();
            try
            {
                if(ModelState.IsValid)
                {
                    var currentUserName = User.Identity.GetUserName();
                    foreach (var person in ctx.People.ToList())
                    {
                        if (person.ApplicationUser.UserName == currentUserName)
                        {
                            medicalOfficeRequest.Person = person;
                            break;
                        }
                    }

                    medicalOfficeRequest.Services = new List<Service>();
                    foreach(var service in selectedServices)
                    {
                        medicalOfficeRequest.Services.Add(ctx.Services.Find(service.Id));
                    }
                    ctx.MedicalOffices.Add(medicalOfficeRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(medicalOfficeRequest);
            }
            catch(Exception e)
            {
                var msg = e.Message;
                return View(medicalOfficeRequest);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                MedicalOffice medicalOffice = ctx.MedicalOffices.Find(id);
                if(medicalOffice == null)
                {
                    return HttpNotFound("Couldn't find the offce with id " + id.ToString() + "!");
                }

                medicalOffice.ServicesList = GetAllServices();
                foreach(var service in medicalOffice.Services)
                {
                    medicalOffice.ServicesList.Find(s => s.Id == service.ServiceId).Checked = true;
                }
                return View(medicalOffice);
            }
            return HttpNotFound("Missing office id parameter!");
        }

        [Authorize]
        [HttpPut]
        public ActionResult Edit(int id, MedicalOffice medicalOfficeRequest)
        {
            MedicalOffice medicalOffice = ctx.MedicalOffices.Find(id);
            var selectedServices = medicalOfficeRequest.ServicesList.Where(s => s.Checked).ToList();

            try
            {
                if(ModelState.IsValid)
                {
                    if(TryUpdateModel(medicalOffice))
                    {
                        medicalOffice.Name = medicalOfficeRequest.Name;
                        medicalOffice.ContactNumber = medicalOfficeRequest.ContactNumber;
                        medicalOffice.Address = medicalOfficeRequest.Address;

                        medicalOffice.Services.Clear();
                        medicalOffice.Services = new List<Service>();
                        foreach(var service in selectedServices)
                        {
                            medicalOffice.Services.Add(ctx.Services.Find(service.Id));
                        }

                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(medicalOfficeRequest);
            }
            catch (Exception)
            {
                return View(medicalOfficeRequest);
            }
        }

        [Authorize]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            MedicalOffice medicalOffice = ctx.MedicalOffices.Find(id);
            if(medicalOffice != null)
            {
                ctx.MedicalOffices.Remove(medicalOffice);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the book with id " + id.ToString() + "!");
        }

        [NonAction]
        public List<CheckBoxViewModel> GetAllServices()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach(var service in ctx.Services.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = service.ServiceId,
                    Name = service.Name,
                    Checked = false
                });
            }
            return checkboxList;
        }
    }
}