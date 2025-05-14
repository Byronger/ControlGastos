using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Models
{
    [Table("Depositos")]
    public class Depositos
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }

        [Required]
        [Display(Name = "Fondo Monetario")]
        public int FondoMonetarioId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        
        [ForeignKey("FondoMonetarioId")]
        public virtual FondosMonetarios FondosMonetarios { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}
