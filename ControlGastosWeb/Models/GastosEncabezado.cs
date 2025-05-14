using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlGastosWeb.Models
{
    [Table("GastosEncabezado")]
    public class GastosEncabezado
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Fondo Monetario")]
        public int FondoMonetarioId { get; set; }

        [StringLength(300)]
        public string Observaciones { get; set; }

        [Display(Name = "Comercio")]
        public string NombreComercio { get; set; }

        [Display(Name = "Tipo de Documento")]
        public string TipoDocumento { get; set; }

        [ForeignKey("FondoMonetarioId")]
        public virtual FondosMonetarios FondosMonetarios { get; set; }

        public virtual ICollection<GastosDetalle> GastosDetalles { get; set; }
    }

}