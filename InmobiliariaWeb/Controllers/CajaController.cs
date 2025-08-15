using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Ingresos;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class CajaController:Controller
    {
        private readonly ICajaService _cajaService;
        private readonly IContratosService _contratosService;
        public CajaController(ICajaService cajaService, IContratosService contratosService)
        {
            _cajaService = cajaService;
            _contratosService = contratosService;
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
                var ingresosIndexTablaDTO = await _cajaService.IngresosIndex(ingresosIndexFilterDTO);/*para el filtro de tipos de ingresos*/

                var ingresosIndexViewModel = new IngresosIndexViewModel { 
                    dFechaDesde = ingresosIndexFilterDTO.dFechaDesde,
                    dFechaHasta = ingresosIndexFilterDTO.dFechaHasta,
                    lIngresosTabla = ingresosIndexTablaDTO
                };

                ingresosIndexViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();/*para el filtro de programas*/

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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetManzanas(int programaId)
        {
            var manzanas = await _contratosService.ManzanaCbxListar(programaId);
            return Json(manzanas);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLotes(int manzanaId)
        {
            var lotes = await _contratosService.LoteCbxListar(manzanaId);
            return Json(lotes);
        }
    }
}
