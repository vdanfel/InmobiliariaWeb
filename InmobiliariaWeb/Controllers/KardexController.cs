using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    [Authorize]
    public class KardexController:Controller
    {
        public KardexController() 
        {
        
        }
        [HttpGet]
        public IActionResult Adenda(int nIdent_Contrato)
        {
            ViewData["ActiveTab"] = "Adenda";
            return View();
        }
        [HttpGet]
        public IActionResult Nuevo(int nIdent_Contrato)
        {
            ViewData["ActiveTab"] = "Nuevo";
            return View();
        }
    }
}
