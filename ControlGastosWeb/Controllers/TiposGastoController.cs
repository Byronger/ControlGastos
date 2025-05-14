using System.Linq;
using System.Web.Mvc;
using ControlGastosWeb.Models;
using System.Net;
using System.Data.Entity;

namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class TiposGastoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var lista = db.TiposGasto.ToList();
            return View(lista);
        }

        public ActionResult Create()
        {
            int count = db.TiposGasto.Count() + 1;
            string codigoGenerado = "TG" + count.ToString("D3");

            var nuevoTipoGasto = new TipoGasto
            {
                Codigo = codigoGenerado
            };

            return View(nuevoTipoGasto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoGasto tipoGasto)
        {
            if (ModelState.IsValid)
            {
                db.TiposGasto.Add(tipoGasto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoGasto);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoGasto tipoGasto = db.TiposGasto.Find(id);
            if (tipoGasto == null)
            {
                return HttpNotFound();
            }

            return View(tipoGasto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Codigo,Descripcion")] TipoGasto tipoGasto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoGasto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoGasto);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoGasto tipoGasto = db.TiposGasto.Find(id);
            if (tipoGasto == null)
            {
                return HttpNotFound();
            }

            return View(tipoGasto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoGasto tipoGasto = db.TiposGasto.Find(id);
            db.TiposGasto.Remove(tipoGasto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
