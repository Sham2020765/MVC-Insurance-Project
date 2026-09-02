[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,FullCoverage,Quote")] Insuree insuree)
{
    if (ModelState.IsValid)
    {
        // a. Start with a base price of $50 / month
        decimal monthlyTotal = 50m;

        // Calculate the user's age based on their Date of Birth
        int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
        if (insuree.DateOfBirth.Date > DateTime.Now.AddYears(-age)) age--;

        // b, c, d. Age-based price adjustments
        if (age <= 18)
        {
            monthlyTotal += 100;
        }
        else if (age >= 19 && age <= 25)
        {
            monthlyTotal += 50;
        }
        else if (age >= 26)
        {
            monthlyTotal += 25;
        }

        // e, f. Car Year-based price adjustments
        if (insuree.CarYear < 2000)
        {
            monthlyTotal += 25;
        }
        else if (insuree.CarYear > 2015)
        {
            monthlyTotal += 25;
        }

        // g, h. Car Make & Model adjustments (Porsche / Carrera)
        if (insuree.CarMake.ToLower() == "porsche")
        {
            monthlyTotal += 25;

            if (insuree.CarModel.ToLower().Contains("carrera"))
            {
                monthlyTotal += 25;
            }
        }

        // i. Add $10 for every speeding ticket the user has
        monthlyTotal += (insuree.SpeedingTickets * 10);

        // j. If the user has a DUI, add 25% to the total
        if (insuree.DUI) 
        {
            monthlyTotal = monthlyTotal * 1.25m;
        }

        // k. If it is full coverage, add 50% to the total
        if (insuree.FullCoverage) 
        {
            monthlyTotal = monthlyTotal * 1.50m;
        }

        // Assign the calculated final value to the Quote field
        insuree.Quote = monthlyTotal;

        db.Insurees.Add(insuree);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    return View(insuree);
}
