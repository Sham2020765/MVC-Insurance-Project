using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InsuranceApp.Models;

namespace InsuranceApp.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,Quote,FullCoverage")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                decimal monthlyTotal = 50;
                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth.Date > DateTime.Now.AddYears(-age)) age--;

                if (age <= 18) monthlyTotal += 100;
                else if (age >= 19 && age <= 25) monthlyTotal += 50;
                else if (age >= 26) monthlyTotal += 25;

                if (insuree.CarYear < 2000 || insuree.CarYear > 2015) monthlyTotal += 25;

                if (insuree.CarMake.ToLower() == "porsche")
                {
                    monthlyTotal += 25;
                    if (insuree.CarModel.ToLower() == "carrera") monthlyTotal += 25;
                }

                monthlyTotal += (insuree.SpeedingTickets * 10);

                if (insuree.DUI) monthlyTotal += (monthlyTotal * 0.25m);
                if (insuree.FullCoverage) monthlyTotal += (monthlyTotal * 0.50m);

                insuree.Quote = monthlyTotal;

                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }
    }
}
