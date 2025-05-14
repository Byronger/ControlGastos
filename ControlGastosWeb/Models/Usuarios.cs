using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ControlGastosWeb.Models
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Identificacion { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Telefono { get; set; }
    }
}