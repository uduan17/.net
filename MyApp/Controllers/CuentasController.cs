using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userMananger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CuentasController(UserManager<IdentityUser> userMananger, SignInManager<IdentityUser> signInManager)
        {
            _userMananger = userMananger;
            _signInManager = signInManager;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroUsuarios ru)
        {
            if (ModelState.IsValid)
            {
                var usuario = new AppUsuario
                {
                    UserName = ru.Email,
                    Email = ru.Email,
                    Documento = ru.Documento,
                    Nombre = ru.Nombre
                };

                var resultado = await _userMananger.CreateAsync(usuario, ru.Password);

                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(usuario, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                ValidarErrores(resultado);
            }

            return View(ru);
        }

        private void ValidarErrores(IdentityResult resultado)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            RegistroUsuarios RuVista = new RegistroUsuarios();
            return View(RuVista);
        }

        [HttpGet]
        public IActionResult Acceso()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Acceso(LoginUsuarios lu)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(lu.Email, lu.Password, lu.Rememberme, lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ingreso invalido");
                    return View(lu);
                }
            }
            return View(lu);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
