using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ControlGastosWeb.Models;

namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class FondosMonetariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FondosMonetarios
        public ActionResult Index()
        {
            var fondos = db.FondosMonetarios.ToList();
            return View(fondos);
        }

        // GET: FondosMonetarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FondosMonetarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FondosMonetarios fondo)
        {
            if (ModelState.IsValid)
            {

                db.FondosMonetarios.Add(fondo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fondo);
        }

        // GET: FondosMonetarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var fondo = db.FondosMonetarios.Find(id);
            if (fondo == null) return HttpNotFound();

            return View(fondo);
        }

        // POST: FondosMonetarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FondosMonetarios fondo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fondo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fondo);
        }

        // GET: FondosMonetarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var fondo = db.FondosMonetarios.Find(id);
            if (fondo == null) return HttpNotFound();

            return View(fondo);
        }

        // POST: FondosMonetarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var fondo = db.FondosMonetarios.Find(id);
            db.FondosMonetarios.Remove(fondo);
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