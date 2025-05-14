using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Models
{
    [NotMapped]
    public class Movimientos
    {
        public DateTime Fecha { get; set; }
        public string UsuarioCorreo { get; set; }
        public string TipoMovimiento { get; set; } 
        public string FondoMonetario { get; set; }
        public string Descripcion { get; set; } 
        public decimal Monto { get; set; }
    }
}