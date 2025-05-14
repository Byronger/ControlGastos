using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ControlGastosWeb.Models;
using Microsoft.AspNet.Identity;

namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class GastosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var gastos = db.GastosEncabezado
                    .Include(d => d.FondosMonetarios)
                    .Where(d => d.UsuarioId == userId)
                    .OrderByDescending(d => d.Fecha)
                    .ToList();

                return View(gastos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Index de GastosController: " + ex.InnerException?.Message, ex);
            }
        }


        // GET: Gastos/Create
        public ActionResult Create()
        {
            ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
            ViewBag.TiposGasto = db.TiposGasto.ToList();
            ViewBag.CorreoUsuario = User.Identity.Name; 
            return View();
        }

        // POST: Gastos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GastosEncabezado model, List<GastosDetalle> detalles)
        {
            if (!ModelState.IsValid || detalles == null || !detalles.Any())
            {
                ModelState.AddModelError("", "Debes completar todos los campos y agregar al menos un detalle.");
                ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
                ViewBag.TiposGasto = db.TiposGasto.ToList();
                ViewBag.CorreoUsuario = User.Identity.Name;
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            model.UsuarioId = userId;
            model.GastosDetalles = new List<GastosDetalle>();

            var fechaGasto = model.Fecha;
            int mes = fechaGasto.Month;
            int anio = fechaGasto.Year;

            // Sobregiros encontrados
            List<string> sobregiros = new List<string>();

            foreach (var detalle in detalles)
            {
                var presupuesto = db.Presupuestos.FirstOrDefault(p =>
                    p.UsuarioId == userId &&
                    p.TipoGastoId == detalle.TipoGastoId &&
                    p.Mes == mes &&
                    p.Anio == anio
                );

                if (presupuesto != null)
                {
                    var montoAcumulado = db.GastosDetalle
                        .Where(gd =>
                            gd.GastosEncabezado.UsuarioId == userId &&
                            gd.TipoGastoId == detalle.TipoGastoId &&
                            gd.GastosEncabezado.Fecha.Month == mes &&
                            gd.GastosEncabezado.Fecha.Year == anio
                        )
                        .Select(gd => gd.Monto)
                        .DefaultIfEmpty(0)
                        .Sum();

                    decimal nuevoTotal = montoAcumulado + detalle.Monto;

                    if (nuevoTotal > presupuesto.Monto)
                    {
                        var tipoGastoNombre = db.TiposGasto.Find(detalle.TipoGastoId)?.Descripcion ?? "Tipo de Gasto";
                        sobregiros.Add($"● {tipoGastoNombre}: Presupuesto Q{presupuesto.Monto:N2}, Total nuevo Q{nuevoTotal:N2}");
                    }
                }

                model.GastosDetalles.Add(new GastosDetalle
                {
                    TipoGastoId = detalle.TipoGastoId,
                    Monto = detalle.Monto
                });
            }

            if (sobregiros.Any())
            {
                ModelState.AddModelError("", "⚠️ Presupuesto sobregirado en los siguientes rubros:");
                foreach (var msg in sobregiros)
                {
                    ModelState.AddModelError("", msg);
                }

                ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
                ViewBag.TiposGasto = db.TiposGasto.ToList();
                ViewBag.CorreoUsuario = User.Identity.Name;
                return View(model);
            }

            decimal totalGasto = detalles.Sum(d => d.Monto);

            var fondo = db.FondosMonetarios.FirstOrDefault(f => f.Id == model.FondoMonetarioId);

            if (fondo == null)
            {
                ModelState.AddModelError("", "El fondo monetario seleccionado no existe.");
                ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
                ViewBag.TiposGasto = db.TiposGasto.ToList();
                ViewBag.CorreoUsuario = User.Identity.Name;
                return View(model);
            }

            if (totalGasto > fondo.Saldo)
            {
                ModelState.AddModelError("", $"⚠️ El fondo seleccionado no tiene saldo suficiente. Saldo actual: Q{fondo.Saldo:N2}, Total a gastar: Q{totalGasto:N2}");

                ViewBag.FondosMonetarios = db.FondosMonetarios.ToList();
                ViewBag.TiposGasto = db.TiposGasto.ToList();
                ViewBag.CorreoUsuario = User.Identity.Name;
                return View(model);
            }

            fondo.Saldo -= totalGasto;

            db.GastosEncabezado.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var gasto = db.GastosEncabezado
                .Include(g => g.FondosMonetarios)
                .Include(g => g.GastosDetalles.Select(d => d.TipoGasto))
                .FirstOrDefault(g => g.Id == id);

            if (gasto == null)
                return HttpNotFound();

            ViewBag.CorreoUsuario = User.Identity.Name;
            return View(gasto);
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
