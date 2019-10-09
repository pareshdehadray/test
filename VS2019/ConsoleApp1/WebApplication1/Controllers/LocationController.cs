using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace WebApplication1.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    public class LocationController : Controller
    {
        private hackathon2019Entities db = new hackathon2019Entities();

        // GET: Location
        public ActionResult Index()
        {
            return View(db.cnf_locations.ToList());
        }

        // GET: Location/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cnf_locations cnf_locations = db.cnf_locations.Find(id);
            if (cnf_locations == null)
            {
                return HttpNotFound();
            }
            return View(cnf_locations);
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,location_code,address,city,state,country,region,pin,weight_unit,volume_unit")] cnf_locations cnf_locations)
        {
            if (ModelState.IsValid)
            {
                db.cnf_locations.Add(cnf_locations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cnf_locations);
        }

        // GET: Location/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cnf_locations cnf_locations = db.cnf_locations.Find(id);
            if (cnf_locations == null)
            {
                return HttpNotFound();
            }
            return View(cnf_locations);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,location_code,address,city,state,country,region,pin,weight_unit,volume_unit")] cnf_locations cnf_locations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cnf_locations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cnf_locations);
        }

        // GET: Location/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cnf_locations cnf_locations = db.cnf_locations.Find(id);
            if (cnf_locations == null)
            {
                return HttpNotFound();
            }
            return View(cnf_locations);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            cnf_locations cnf_locations = db.cnf_locations.Find(id);
            db.cnf_locations.Remove(cnf_locations);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
