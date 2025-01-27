using InmobiliariaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class DashboardController:Controller
    {
        public DashboardController() 
        {
        
        }
        [Authorize]
        public IActionResult Index(string Mensaje) 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                dashboardViewModel.Mensaje = Mensaje;
                return View(dashboardViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
    }
}
