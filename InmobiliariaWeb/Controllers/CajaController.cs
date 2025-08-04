using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Ingresos;
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
        public async Task<IActionResult> IngresosIndex() 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var ingresosIndexFilterDTO = new IngresosIndexFilterDTO {
                    dFechaDesde = DateTime.Parse("2025/06/01".ToString()),
                    dFechaHasta = DateTime.Now
                };
                var ingresosIndexTablaDTO = await _cajaService.IngresosIndex(ingresosIndexFilterDTO);

                var ingresosIndexViewModel = new IngresosIndexViewModel { 
                    dFechaDesde = ingresosIndexFilterDTO.dFechaDesde,
                    dFechaHasta = ingresosIndexFilterDTO.dFechaHasta,
                    lIngresosTabla = ingresosIndexTablaDTO
                };

                return View(ingresosIndexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> IngresosIndex(IngresosIndexViewModel ingresosIndexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                return View(ingresosIndexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
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
