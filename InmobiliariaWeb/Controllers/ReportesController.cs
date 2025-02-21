using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class ReportesController:Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult ReporteCliente() 
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult ReporteNotificaciones()
        {
            return View();
        }
    }
}
