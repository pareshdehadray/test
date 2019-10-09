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
    public class TestUserController : Controller
    {
        private hackathon2019Entities db = new hackathon2019Entities();

        // GET: TestUser
        public ActionResult Index()
        {
            var cnf_users = db.cnf_users.Include(c => c.cnf_locations);
            return View(cnf_users.ToList());
        }

        // GET: TestUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cnf_users cnf_users = db.cnf_users.Find(id);
            if (cnf_users == null)
            {
                return HttpNotFound();
            }
            return View(cnf_users);
        }

        // GET: TestUser/Create
        public ActionResult Create()
        {
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code");
            ViewBag.category = new SelectList(db.cnf_categories, "id", "category");

            return View();
        }

        // POST: TestUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,password,first_name,last_name,location_id,role,password_changed")] cnf_users cnf_users)
        {
            if (ModelState.IsValid)
            {
                db.cnf_users.Add(cnf_users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code", cnf_users.location_id);
            return View(cnf_users);
        }

        // GET: TestUser/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cnf_users cnf_users = db.cnf_users.Find(id);
            if (cnf_users == null)
            {
                return HttpNotFound();
            }
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code", cnf_users.location_id);
            return View(cnf_users);
        }

        // POST: TestUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,password,first_name,last_name,location_id,role,password_changed")] cnf_users cnf_users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cnf_users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code", cnf_users.location_id);
            return View(cnf_users);
        }

        // GET: TestUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cnf_users cnf_users = db.cnf_users.Find(id);
            if (cnf_users == null)
            {
                return HttpNotFound();
            }
            return View(cnf_users);
        }

        // POST: TestUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            cnf_users cnf_users = db.cnf_users.Find(id);
            db.cnf_users.Remove(cnf_users);
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
