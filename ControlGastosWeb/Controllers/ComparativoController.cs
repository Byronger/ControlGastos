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
    public class ComparativoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var resultados = new List<Comparativo>();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                var mesInicio = fechaInicio.Value.Month;
                var mesFin = fechaFin.Value.Month;
                var anioInicio = fechaInicio.Value.Year;
                var anioFin = fechaFin.Value.Year;

                var presupuestos = db.Presupuestos
                    .Include(p => p.TipoGasto)
                    .Where(p =>
                        (p.Anio > anioInicio || (p.Anio == anioInicio && p.Mes >= mesInicio)) &&
                        (p.Anio < anioFin || (p.Anio == anioFin && p.Mes <= mesFin))
                    )
                    .GroupBy(p => p.TipoGastoId)
                    .Select(g => new
                    {
                        TipoGastoId = g.Key,
                        TipoGastoNombre = g.FirstOrDefault().TipoGasto.Descripcion,
                        TotalPresupuestado = g.Sum(x => x.Monto)
                    })
                    .ToList();

                var gastos = db.GastosDetalle
                    .Include(g => g.TipoGasto)
                    .Include(g => g.GastosEncabezado)
                    .Where(g =>
                        g.GastosEncabezado.Fecha >= fechaInicio &&
                        g.GastosEncabezado.Fecha <= fechaFin
                    )
                    .GroupBy(g => g.TipoGastoId)
                    .Select(g => new
                    {
                        TipoGastoId = g.Key,
                        TipoGastoNombre = g.FirstOrDefault().TipoGasto.Descripcion,
                        TotalEjecutado = g.Sum(x => x.Monto)
                    })
                    .ToList();

                var todosTipoGasto = presupuestos.Select(p => p.TipoGastoId)
                                    .Union(gastos.Select(g => g.TipoGastoId))
                                    .Distinct();

                foreach (var tipoId in todosTipoGasto)
                {
                    var presupuesto = presupuestos.FirstOrDefault(p => p.TipoGastoId == tipoId);
                    var ejecucion = gastos.FirstOrDefault(g => g.TipoGastoId == tipoId);

                    resultados.Add(new Comparativo
                    {
                        TipoGasto = presupuesto?.TipoGastoNombre ?? ejecucion?.TipoGastoNombre ?? "Desconocido",
                        Presupuestado = presupuesto?.TotalPresupuestado ?? 0,
                        Ejecutado = ejecucion?.TotalEjecutado ?? 0
                    });
                }
            }

            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");

            return View(resultados);
        }
    }
}