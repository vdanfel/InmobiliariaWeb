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
        private readonly ITablasService _tablasService;
        public CajaController(ICajaService cajaService, IContratosService contratosService, ITablasService tablasService)
        {
            _cajaService = cajaService;
            _contratosService = contratosService;
            _tablasService = tablasService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IngresosIndex() 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var ingresosIndexFilterDTO = new IngresosIndexFilterDTO {
                    dFechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    dFechaHasta = DateTime.Now
                };

                var ingresosIndexTablaDTO = await _cajaService.IngresosIndex(ingresosIndexFilterDTO);/*para llenar los datos de la caja con los filtros*/

                var ingresosIndexViewModel = new IngresosIndexViewModel { 
                    dFechaDesde = ingresosIndexFilterDTO.dFechaDesde,
                    dFechaHasta = ingresosIndexFilterDTO.dFechaHasta,
                    lIngresosTabla = ingresosIndexTablaDTO,
                    nTotalSoles = ingresosIndexTablaDTO
                .Where(x => x.nIdent_002_TipoMoneda == 24)
                .Sum(x => x.nImporte),
                    nTotalDolares = ingresosIndexTablaDTO
                .Where(x => x.nIdent_002_TipoMoneda == 23)
                .Sum(x => x.nImporte)
                };

                ingresosIndexViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();/*para el filtro de programas*/
                ingresosIndexViewModel.lTipoIngreso = await _tablasService.ListarTipoIngreso();
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

                IngresosIndexFilterDTO ingresosIndexFilterDTO = new IngresosIndexFilterDTO
                {
                    dFechaDesde = ingresosIndexViewModel.dFechaDesde,
                    dFechaHasta = ingresosIndexViewModel.dFechaHasta,
                    nIdent_021_TipoIngresos = ingresosIndexViewModel.nIdent_021_TipoIngresos,
                    nIdent_Programa = ingresosIndexViewModel.Ident_Programa,
                    nIdent_Manzana = ingresosIndexViewModel.Ident_Manzana,
                    nIdent_Lote = ingresosIndexViewModel.Ident_Lote,
                    nIdent_Persona = ingresosIndexViewModel.Ident_Persona
                };

                var ingresosIndexTablaDTO = await _cajaService.IngresosIndex(ingresosIndexFilterDTO);
                ingresosIndexViewModel.lIngresosTabla = ingresosIndexTablaDTO;
                ingresosIndexViewModel.nTotalSoles = ingresosIndexTablaDTO
                    .Where(x => x.nIdent_002_TipoMoneda == 24)
                    .Sum(x => x.nImporte);
                ingresosIndexViewModel.nTotalDolares = ingresosIndexTablaDTO
                    .Where(x => x.nIdent_002_TipoMoneda == 23)
                    .Sum(x => x.nImporte);

                ingresosIndexViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();
                ingresosIndexViewModel.lTipoIngreso = await _tablasService.ListarTipoIngreso();

                return View(ingresosIndexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IngresoNuevo()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

                IngresosViewModel ingresosViewModel = new IngresosViewModel();
                ingresosViewModel.lProgramasCbxLists = await _contratosService.ProgramaCbxListar();
                var tipoIngresosLista = await _tablasService.ListarTipoIngreso();

                ingresosViewModel.lTipoIngreso = tipoIngresosLista
                    .Where(x => x.nIdent_021_TipoIngreso != 135
                             && x.nIdent_021_TipoIngreso != 136
                             && x.nIdent_021_TipoIngreso != 137)
                    .ToList();
                ingresosViewModel.lBancos = await _tablasService.ListarBancos();
                ingresosViewModel.lTipoPagos = await _tablasService.ListarTipoPago();
                ingresosViewModel.lTipoMonedas = await _tablasService.ListarTipoMoneda();
                ingresosViewModel.dFechaIngreso = DateTime.Now;
                return View(ingresosViewModel);
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
