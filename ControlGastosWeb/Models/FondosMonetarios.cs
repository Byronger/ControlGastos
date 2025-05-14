using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ControlGastosWeb.Models
{
    [Table("FondosMonetarios")]
    public class FondosMonetarios
    {
        public int Id { get; set; }
        [Required]
        public string NombreBanco { get; set; }
        [Required]
        public decimal NoCuenta { get; set; }
        [Required]
        public decimal Saldo { get; set; }
    }

}