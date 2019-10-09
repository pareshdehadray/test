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
    //[Authorize(Roles = "Vendor")]
    [Authorize]
    public class VendorController : Controller
    {
        private hackathon2019Entities db = new hackathon2019Entities();

        // GET: Vendor
        public ActionResult Index()
        {
            string userName = Session["UserName"].ToString();
            var item_utilisation = db.item_utilisation.Where(x => x.created_by == userName).Include(i => i.cnf_categories).Include(i => i.cnf_locations).Include(i => i.cnf_users);
            return View(item_utilisation.ToList());
        }

        // GET: Vendor/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item_utilisation item_utilisation = db.item_utilisation.Find(id);
            if (item_utilisation == null)
            {
                return HttpNotFound();
            }
            return View(item_utilisation);
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            string userName = Session["UserName"].ToString();
            var userCategories = db.cnf_user_category_mapping.Where(x => x.username == userName).Join(db.cnf_categories, x => x.category_id, y => y.id, (x, y) => new { id= y.id, category = y.category });
            var list = userCategories.ToList();
            
            ViewBag.category_id = new SelectList(userCategories, "id", "category");
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code");
            ViewBag.created_by = new SelectList(db.cnf_users, "username", "username");

            item_utilisation item_Utilisation = new item_utilisation();
            item_Utilisation.asof_date = DateTime.Now.Date;

            return View(item_Utilisation);
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,asof_date,category_id,acquired,unused,wasted,created_by,location_id")] item_utilisation item_utilisation)
        {
            if (ModelState.IsValid)
            {
                string userName = Session["UserName"].ToString();
                var user = db.cnf_users.Where(x => x.username == userName).First();
                item_utilisation.created_by = user.username;
                item_utilisation.location_id = user.location_id;

                db.item_utilisation.Add(item_utilisation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.cnf_categories, "id", "category", item_utilisation.category_id);
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code", item_utilisation.location_id);
            ViewBag.created_by = new SelectList(db.cnf_users, "username", "password", item_utilisation.created_by);
            return View(item_utilisation);
        }

        // GET: Vendor/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item_utilisation item_utilisation = db.item_utilisation.Find(id);
            if (item_utilisation == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.cnf_categories, "id", "category", item_utilisation.category_id);
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code", item_utilisation.location_id);
            ViewBag.created_by = new SelectList(db.cnf_users, "username", "password", item_utilisation.created_by);
            return View(item_utilisation);
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,asof_date,category_id,acquired,unused,wasted,created_by,location_id")] item_utilisation item_utilisation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_utilisation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.cnf_categories, "id", "category", item_utilisation.category_id);
            ViewBag.location_id = new SelectList(db.cnf_locations, "id", "location_code", item_utilisation.location_id);
            ViewBag.created_by = new SelectList(db.cnf_users, "username", "password", item_utilisation.created_by);
            return View(item_utilisation);
        }

        // GET: Vendor/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item_utilisation item_utilisation = db.item_utilisation.Find(id);
            if (item_utilisation == null)
            {
                return HttpNotFound();
            }
            return View(item_utilisation);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            item_utilisation item_utilisation = db.item_utilisation.Find(id);
            db.item_utilisation.Remove(item_utilisation);
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
