using ControlGastosWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class PresupuestosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Presupuestos
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            //var userIdString = User.Identity.GetUserId();
            //Guid userId;
            var userId = User.Identity.GetUserId(); // string

            
            var presupuestos = db.Presupuestos
                .Include(p => p.TipoGasto)
                .Where(p => p.UsuarioId == userId)
                .OrderByDescending(p => p.Anio).ThenByDescending(p => p.Mes)
                .ToList();

            return View(presupuestos);
        }


        // GET: Presupuestos/Create
        // GET: Presupuestos/Create
        public ActionResult Create()
        {
            ViewBag.TiposGasto = db.TiposGasto.ToList();

            ViewBag.Mes = Enumerable.Range(1, 12)
                .Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = new DateTime(1, i, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"))
                }).ToList();

            return View();
        }

        // POST: Presupuestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Presupuestos presupuesto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                presupuesto.UsuarioId = userId;

                db.Presupuestos.Add(presupuesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TiposGasto = db.TiposGasto.ToList();

            ViewBag.Mes = Enumerable.Range(1, 12)
                .Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = new DateTime(1, i, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES"))

                }).ToList();



            foreach (var modelError in ModelState)
            {
                foreach (var error in modelError.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Error en {modelError.Key}: {error.ErrorMessage}");
                }
            }

            return View(presupuesto);
        }

        // GET: Presupuestos/Edit/5
        public ActionResult Edit(int id)
        {
            var presupuesto = db.Presupuestos.Find(id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }

            ViewBag.TiposGasto = db.TiposGasto.ToList();

            var meses = Enumerable.Range(1, 12).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = new DateTime(1, i, 1).ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
            }).ToList();

            ViewBag.Mes = new SelectList(meses, "Value", "Text", presupuesto.Mes.ToString());

            return View(presupuesto);
        }

        // POST: Presupuestos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Presupuestos presupuesto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                presupuesto.UsuarioId = userId;

                db.Entry(presupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TiposGasto = db.TiposGasto.ToList();
            ViewBag.Mes = Enumerable.Range(1, 12)
                .Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                }).ToList();

            return View(presupuesto);
        }

        // GET: Presupuestos/Delete/5
        public ActionResult Delete(int id)
        {
            var presupuesto = db.Presupuestos.Find(id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }
            return View(presupuesto);
        }

        // POST: Presupuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var presupuesto = db.Presupuestos.Find(id);
            db.Presupuestos.Remove(presupuesto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
