using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Models
{
    [Table("GastosDetalle")]
    public class GastosDetalle
    {
        public int Id { get; set; }

        public int GastoEncabezadoId { get; set; }

        [Required]
        [Display(Name = "Tipo de Gasto")]
        public int TipoGastoId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        [ForeignKey("TipoGastoId")]
        public virtual TipoGasto TipoGasto { get; set; }

        [ForeignKey("GastoEncabezadoId")]
        public virtual GastosEncabezado GastosEncabezado { get; set; }
    }

}