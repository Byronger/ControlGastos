using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Models
{
    public class Presupuestos
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }

        [Required]
        [Display(Name = "Tipo de Gasto")]
        public int TipoGastoId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12.")]
        [Display(Name = "Mes")]
        public short Mes { get; set; }

        [Required]
        [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100.")]
        [Display(Name = "Año")]
        public short Anio { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [ForeignKey("TipoGastoId")]
        public virtual TipoGasto TipoGasto { get; set; }
    }
}
