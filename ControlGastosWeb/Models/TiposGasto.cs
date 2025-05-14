using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ControlGastosWeb.Models
{
    [Table("TiposGasto")]
    public class TipoGasto
    {
        public int ID { get; set; }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Descripcion { get; set; }
    }
}