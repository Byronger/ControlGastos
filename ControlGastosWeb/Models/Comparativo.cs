using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Models
{
    [NotMapped]
    public class Comparativo
    {
        public string TipoGasto { get; set; }
        public decimal Presupuestado { get; set; }
        public decimal Ejecutado { get; set; }
    }
}