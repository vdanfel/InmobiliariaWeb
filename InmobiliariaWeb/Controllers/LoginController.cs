using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Result;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InmobiliariaWeb.Controllers
{
    public class LoginController:Controller
    {
        private readonly ILoginService _loginService;
        private readonly ICajaService _cajaService;
        public LoginController(ILoginService loginService, ICajaService cajaService) 
        { 
            _loginService = loginService;
            _cajaService = cajaService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // La variable de sesión está presente, redirigir a Dashboard/Index
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else 
            {
                // Borra los claims existentes
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            // El usuario no está autenticado, mostrar la vista de inicio de sesión
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            LoginResult loginResult = await _loginService.ValidarLogin(loginViewModel.Usuario, loginViewModel.Clave);
            if (!string.IsNullOrEmpty(loginResult.Mensaje))
            {
                HttpContext.Session.SetInt32("IdentUsuario", loginResult.IdentUsuario);
                HttpContext.Session.SetString("Usuario", loginResult.Usuario);
                HttpContext.Session.SetInt32("Ident005TipoUsuario", loginResult.Ident005TipoUsuario);

                // Iniciar sesión
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResult.Usuario),
                    new Claim("IdentUsuario", loginResult.IdentUsuario.ToString()),
                    // Agrega más claims según tus necesidades
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Para que la cookie de autenticación persista incluso después de cerrar el navegador
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                var Ejecutado = await _loginService.CargasIniciales();
                if (Ejecutado == false)
                {
                    await _loginService.AnularSeparacionesVencidas();
                    await _loginService.ActualizarDiasMoras();
                    await _loginService.ActualizarTotalesKardex();
                }
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                loginViewModel.Mensaje = loginResult.Mensaje;
                return View(loginViewModel);
            }
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult Alerta(string Mensaje)
        {
            ViewBag.Mensaje = Mensaje;
            return View();
        }
    }
}
