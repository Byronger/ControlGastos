using ControlGastosWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class DepositosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Depositos
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var depositos = db.Depositos
                .Include(d => d.FondosMonetarios)
                .Where(d => d.UsuarioId == userId)
                .OrderByDescending(d => d.Fecha)
                .ToList();

            return View(depositos);
        }

        // GET: Depositos/Create
        public ActionResult Create()
        {
            ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
            var correoUsuario = User.Identity.GetUserName();
            ViewBag.CorreoUsuario = correoUsuario;

            return View();
        }

        // POST: Depositos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Depositos deposito)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                deposito.UsuarioId = userId;

                // Buscar el fondo monetario seleccionado
                var fondo = db.FondosMonetarios.FirstOrDefault(f => f.Id == deposito.FondoMonetarioId);
                if (fondo == null)
                {
                    ModelState.AddModelError("", "Fondo monetario no válido.");
                    ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
                    return View(deposito);
                }

                // Sumar el monto del depósito al saldo del fondo
                fondo.Saldo += deposito.Monto;

                // Guardar depósito y actualizar fondo
                db.Depositos.Add(deposito);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
            return View(deposito);
        }


        // GET: Depositos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var deposito = db.Depositos.Find(id);
            if (deposito == null)
                return HttpNotFound();

            if (deposito.UsuarioId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
            return View(deposito);
        }

        // POST: Depositos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Depositos deposito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deposito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
            return View(deposito);
        }

        // GET: Depositos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var deposito = db.Depositos
                .Include(d => d.FondosMonetarios)
                .FirstOrDefault(d => d.Id == id);

            if (deposito == null)
                return HttpNotFound();

            if (deposito.UsuarioId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(deposito);
        }

        // POST: Depositos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var deposito = db.Depositos.Find(id);
            if (deposito != null && deposito.UsuarioId == User.Identity.GetUserId())
            {
                db.Depositos.Remove(deposito);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
