using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TiposGasto()
        {
            ViewBag.Message = "Tipos de Gasto";

            return View();
        }

        public ActionResult Usuarios()
        {
            ViewBag.Message = "Usuarios";

            return View();
        }

        public ActionResult FondoMonetario()
        {
            ViewBag.Message = "Fondo Monetario";

            return View();
        }
    }
}