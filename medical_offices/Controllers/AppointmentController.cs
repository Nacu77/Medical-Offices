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
    [Authorize]
    public class AppointmentController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            List<Person> people = ctx.People.ToList();
            Person person = null;
            foreach (var p in people)
            {
                if (p.ApplicationUser.UserName == userName)
                {
                    person = p;
                    break;
                }
            }

            ViewBag.Appointments = person.Appointments;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id.HasValue)
            {
                Appointment appointment = ctx.Appointments.Find(id);
                if(appointment != null)
                {
                    return View(appointment);
                }
                return HttpNotFound("Coulnd't find appointment with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing appointment id parameter!");
        }

        [HttpGet]
        public ActionResult New(int? id)
        {
            if(id.HasValue)
            {
                MedicalOffice medicalOffice = ctx.MedicalOffices.Find(id);
                if(medicalOffice == null)
                {
                    return HttpNotFound("Couldn't find medical office with id " + id.ToString() + "!");
                }
                Appointment appointment = new Appointment();
                appointment.MedicalOffice = medicalOffice;
                return View(appointment);
            }
            return HttpNotFound("Missing medical office id parameter!");
        }

        [HttpPost]
        public ActionResult New(int id, Appointment appointmentRequest)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var userName = User.Identity.GetUserName();
                    List<Person> people = ctx.People.ToList();
                    Person person = null;
                    foreach(var p in people)
                    {
                        if(p.ApplicationUser.UserName == userName)
                        {
                            person = p;
                            break;
                        }
                    }

                    appointmentRequest.Person = person;
                    appointmentRequest.MedicalOffice = ctx.MedicalOffices.Find(id);
                    ctx.Appointments.Add(appointmentRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(appointmentRequest);
            }
            catch(Exception)
            {
                return View(appointmentRequest);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Appointment appointment = ctx.Appointments.Find(id);
                if(appointment == null)
                {
                    return HttpNotFound("Couldn't find the appointment with id " + id.ToString() + "!");
                }
                return View(appointment);
            }
            return HttpNotFound("Missing appointment id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Appointment appointmentRequest)
        {
            Appointment appointment = ctx.Appointments.Find(id);
            try
            {
                if(ModelState.IsValid)
                {
                    if(TryUpdateModel(appointment))
                    {
                        appointment.Date = appointmentRequest.Date;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(appointmentRequest);
            }
            catch(Exception)
            {
                return View(appointmentRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Appointment appointment = ctx.Appointments.Find(id);
            if(appointment != null)
            {
                ctx.Appointments.Remove(appointment);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the appointment with id " + id.ToString() + "!");
        }
    }
}