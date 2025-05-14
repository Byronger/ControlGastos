using System.Linq;
using System.Web.Mvc;
using ControlGastosWeb.Models;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;


namespace ControlGastosWeb.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //var lista = db.UsuariosApp.ToList();
            var usuarios = db.Users
    .Select(u => new UsuarioViewModel
    {
        Id = u.Id,
        Nombre = u.Nombre,
        Apellido = u.Apellido,
        Email = u.Email,
        UserName = u.UserName,
        FechaNacimiento = u.FechaNacimiento,
        Direccion = u.Direccion,
        PhoneNumber = u.PhoneNumber
    }).ToList();

            return View(usuarios);
        }

        public ActionResult Create()
        {
            return View();
        }

        /*public ActionResult Create(Usuarios usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
                {
                    ModelState.AddModelError("PasswordHash", "La contraseña es obligatoria.");
                }

                if (ModelState.IsValid)
                {
                    var hasher = new PasswordHasher();
                    usuario.PasswordHash = hasher.HashPassword(usuario.PasswordHash);

                    db.UsuariosApp.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string msg = $"Error en propiedad: {validationError.PropertyName} - {validationError.ErrorMessage}";
                        System.Diagnostics.Debug.WriteLine(msg); // Esto aparece en la ventana "Output" de Visual Studio

                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


            return View(usuario);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    var usuario = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        FechaNacimiento = model.FechaNacimiento,
                        Direccion = model.Direccion,
                        PhoneNumber = model.PhoneNumber,
                        Correo = model.Login,
                    };

                    var result = userManager.Create(usuario, model.PasswordHash);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
            }

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = userManager.FindById(id);

                if (user == null)
                    return HttpNotFound();

                var viewModel = new UsuarioViewModel
                {
                    UserName = user.UserName,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    FechaNacimiento = user.FechaNacimiento,
                    Direccion = user.Direccion,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Login = user.Correo
                };

                ViewBag.UserId = user.Id;
                return View(viewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UsuarioViewModel model)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!ModelState.IsValid)
                return View(model);

            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = userManager.FindById(id);

                if (user == null)
                    return HttpNotFound();

                user.UserName = model.UserName;
                user.Nombre = model.Nombre;
                user.Apellido = model.Apellido;
                user.FechaNacimiento = model.FechaNacimiento;
                user.Direccion = model.Direccion;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Correo = model.Login;

                if (!string.IsNullOrWhiteSpace(model.PasswordHash))
                {
                    var token = userManager.GeneratePasswordResetToken(user.Id);
                    var resultPwd = userManager.ResetPassword(user.Id, token, model.PasswordHash);

                    if (!resultPwd.Succeeded)
                    {
                        foreach (var err in resultPwd.Errors)
                            ModelState.AddModelError("", err);
                        return View(model);
                    }
                }

                var result = userManager.Update(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                return View(model);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var usuario = await userManager.FindByIdAsync(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                return View(usuario);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var usuario = await userManager.FindByIdAsync(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var result = await userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View("Delete", usuario);
                }

                return RedirectToAction("Index");
            }
        }


    }
}
