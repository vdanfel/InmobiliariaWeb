using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class CajaController:Controller
    {
        private readonly ICajaService _cajaService;
        public CajaController(ICajaService cajaService)
        {
            _cajaService = cajaService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult IngresosIndex() 
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult EgresosIndex()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCuentasBancarias(int Ident_019_banco)
        {
            var cuentasBancariasList = await _cajaService.CuentasBancariasXBanco(Ident_019_banco);
            return Json(cuentasBancariasList);
        }
       
    }
}
