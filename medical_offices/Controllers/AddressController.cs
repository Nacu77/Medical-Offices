using medical_offices.Models;
using medical_offices.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace medical_offices.Controllers
{
    public class AddressController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Addresses = ctx.Addresses.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id.HasValue)
            {
                Address adress = ctx.Addresses.Find(id);
                if(adress != null)
                {
                    List<MedicalOffice> medicalOffices = ctx.MedicalOffices.ToList();
                    foreach(var office in medicalOffices)
                    {
                        if(office.Address.AddressId == id)
                        {
                            ViewBag.MedicalOffice = office;
                            break;
                        }
                    }

                    return View(adress);
                }
                return HttpNotFound("Couldn't find the adress with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing adress id parameter!");
        }

        [HttpGet]
        public ActionResult New()
        {
            Address address = new Address();
            return View(address);
        }

        [HttpPost]
        public ActionResult New(Address addressRequest)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    ctx.Addresses.Add(addressRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(addressRequest);
            }
            catch(Exception)
            {
                return View(addressRequest);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Address address = ctx.Addresses.Find(id);
                if(address == null)
                {
                    return HttpNotFound("Couldn't find the adress with id " + id.ToString() + "!");
                }
                return View(address);
            }
            return HttpNotFound("Missing address id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Address addressRequest)
        {
            Address address = ctx.Addresses.Find(id);
            try
            {
                if(ModelState.IsValid)
                {
                    if(TryUpdateModel(address))
                    {
                        address.City = addressRequest.City;
                        address.Country = addressRequest.Country;
                        address.Street = addressRequest.Street;
                        address.Number = addressRequest.Number;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(addressRequest);
            }
            catch(Exception)
            {
                return View(addressRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Address address = ctx.Addresses.Find(id);
            if(address != null)
            {
                ctx.Addresses.Remove(address);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the address with id " + id.ToString() + "!");
        }
    }
}