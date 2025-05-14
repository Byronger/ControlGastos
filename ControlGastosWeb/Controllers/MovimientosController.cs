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
    public class MovimientosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<Movimientos> movimientos = new List<Movimientos>();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                var gastos = db.GastosEncabezado
                    .Include(g => g.FondosMonetarios)
                    .Where(g => g.Fecha >= fechaInicio && g.Fecha <= fechaFin)
                    .ToList();

                foreach (var gasto in gastos)
                {
                    var usuario = db.Users.FirstOrDefault(u => u.Id == gasto.UsuarioId);
                    var montoTotal = gasto.GastosDetalles.Sum(d => d.Monto);

                    movimientos.Add(new Movimientos
                    {
                        Fecha = gasto.Fecha,
                        UsuarioCorreo = usuario?.Email ?? gasto.UsuarioId,
                        TipoMovimiento = "Gasto",
                        FondoMonetario = gasto.FondosMonetarios?.NombreBanco,
                        Descripcion = gasto.NombreComercio ?? gasto.Observaciones,
                        Monto = montoTotal
                    });
                }

                var depositos = db.Depositos
                    .Include(d => d.FondosMonetarios)
                    .Where(d => d.Fecha >= fechaInicio && d.Fecha <= fechaFin)
                    .ToList();

                foreach (var deposito in depositos)
                {
                    var usuario = db.Users.FirstOrDefault(u => u.Id == deposito.UsuarioId);

                    movimientos.Add(new Movimientos
                    {
                        Fecha = deposito.Fecha,
                        UsuarioCorreo = usuario?.Email ?? deposito.UsuarioId,
                        TipoMovimiento = "Depósito",
                        FondoMonetario = deposito.FondosMonetarios?.NombreBanco,
                        Descripcion = "Depósito registrado",
                        Monto = deposito.Monto
                    });
                }
                movimientos = movimientos.OrderByDescending(m => m.Fecha).ToList();
            }

            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");

            return View(movimientos);
        }
    }
}