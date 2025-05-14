using System;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ControlGastosWeb.Models; // Ajustá a tu namespace

[assembly: OwinStartupAttribute(typeof(ControlGastosWeb.Startup))]
namespace ControlGastosWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

    public class DbInitializer
    {
        public static void CrearUsuarioInicial()
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                string email = "admin@smartbit.com";
                string password = "Admin123!";

                // Verificar si ya existe
                if (userManager.FindByEmail(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        Nombre = "Administrador",
                        Apellido = "SMARTBIT",
                        FechaNacimiento = new DateTime(1990, 1, 1),
                        Direccion = "Oficinas SMARTBIT",
                        PhoneNumber = "1234567890"
                    };

                    var result = userManager.Create(user, password);
                    if (!result.Succeeded)
                    {
                        // Log errores si hay
                        var errores = string.Join(", ", result.Errors);
                        throw new Exception("Error al crear usuario: " + errores);
                    }
                }
            }
        }
    }

}
