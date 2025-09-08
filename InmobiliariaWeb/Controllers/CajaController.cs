using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Models.Egresos;
using InmobiliariaWeb.Models.Ingresos;
using InmobiliariaWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    [Authorize]
    public class CajaController:Controller
    {
        private readonly ICajaService _cajaService;
        private readonly IContratosService _contratosService;
        private readonly ITablasService _tablasService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CajaController(ICajaService cajaService, IContratosService contratosService, ITablasService tablasService, IWebHostEnvironment webHostEnvironment)
        {
            _cajaService = cajaService;
            _contratosService = contratosService;
            _tablasService = tablasService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> IngresosIndex() 
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            var ingresosIndexFilterDTO = new IngresosIndexFilterDTO
            {
                dFechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                dFechaHasta = DateTime.Now
            };

            var ingresosIndexTablaDTO = await _cajaService.IngresosIndex(ingresosIndexFilterDTO);/*para llenar los datos de la caja con los filtros*/

            var ingresosIndexViewModel = new IngresosIndexViewModel
            {
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
        [HttpPost]
        public async Task<IActionResult> IngresosIndex(IngresosIndexViewModel ingresosIndexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
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
        [HttpGet]
        public async Task<IActionResult> IngresoNuevo()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
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
            ingresosViewModel.lTipoMonedasCabecera = await _tablasService.ListarTipoMoneda();
            ingresosViewModel.dFechaIngreso = DateTime.Now;
            return View(ingresosViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> IngresoNuevo(IngresosViewModel ingresosViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            IngresosModel ingresosModel = new IngresosModel
            {
                FechaPago = ingresosViewModel.dFechaIngreso,
                Ident_002_TipoMoneda = ingresosViewModel.nIdent_002_TipoMonedaCabecera,
                ImporteTotal = ingresosViewModel.nImporte,
                Ident_021_TipoIngresos = ingresosViewModel.nIdent_021_TipoIngresos,
                Ident_Persona = ingresosViewModel.nIdent_Persona,
                Observacion = ingresosViewModel.sObservacion,
                Ident_Programa = ingresosViewModel.nIdent_Programa,
                Ident_Manzana = ingresosViewModel.nIdent_Manzana,
                Ident_Lote = ingresosViewModel.nIdent_Lote,
                Ident_015_EstadoPago = 110
            };
            if (ingresosViewModel.nIdent_021_TipoIngresos != 139)
            {
                ingresosModel.Ident_Origen = ingresosViewModel.nIdent_Lote;
            }
            ingresosViewModel.nIdent_Ingresos = await _cajaService.Ingresos_Insert(ingresosModel, loginResult);

            IngresosDetalleModel ingresosDetalleModel = new IngresosDetalleModel
            {
                Ident_Ingresos = ingresosViewModel.nIdent_Ingresos,
                TipoCambio = ingresosViewModel.nTipoCambio,
                Ident_018_TipoPago = ingresosViewModel.nIdent_018_TipoPago,
                Ident_CuentasBancarias = ingresosViewModel.nIdent_CuentasBancarias,
                Ident_002_TipoMoneda = ingresosViewModel.nIdent_002_TipoMoneda,
                Importe = ingresosViewModel.nImporte,
                NumeroOperacion = ingresosViewModel.sNumeroOperacion,
                Fecha = ingresosViewModel.dFechaIngreso,
            };

            if (ingresosModel.Ident_002_TipoMoneda == 24)
            {
                if (ingresosDetalleModel.Ident_002_TipoMoneda == ingresosModel.Ident_002_TipoMoneda)
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte;
                }
                else
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte * ingresosViewModel.nTipoCambio;
                }
            }
            else
            {
                if (ingresosDetalleModel.Ident_002_TipoMoneda == ingresosModel.Ident_002_TipoMoneda)
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte;
                }
                else
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte / ingresosViewModel.nTipoCambio;
                }
            }

            ingresosDetalleModel.Ident_IngresosDetalle = await _cajaService.IngresosDetalle_Insert(ingresosDetalleModel, loginResult);
            return RedirectToAction("IngresoVer", "Caja", new { nIdent_Ingresos = ingresosViewModel.nIdent_Ingresos });
        }
        [HttpGet]
        public async Task<IActionResult> IngresoVer(int nIdent_Ingresos, string sMensaje)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            IngresosViewModel ingresosViewModel = new IngresosViewModel();
            ingresosViewModel = await _cajaService.IngresosSelect(nIdent_Ingresos);
            ingresosViewModel.lProgramasCbxLists = await _contratosService.ProgramaCbxListar();
            ingresosViewModel.lTipoIngreso = await _tablasService.ListarTipoIngreso();
            ingresosViewModel.lBancos = await _tablasService.ListarBancos();
            ingresosViewModel.lTipoPagos = await _tablasService.ListarTipoPago();
            ingresosViewModel.lTipoMonedas = await _tablasService.ListarTipoMoneda();
            ingresosViewModel.lTipoMonedasCabecera = await _tablasService.ListarTipoMoneda();
            ingresosViewModel.lIngresosDetallesList = await _cajaService.IngresosDetalle_List(nIdent_Ingresos);

            return View(ingresosViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> IngresoVer(IngresosViewModel ingresosViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            IngresosDetalleModel ingresosDetalleModel = new IngresosDetalleModel
            {
                Ident_Ingresos = ingresosViewModel.nIdent_Ingresos,
                TipoCambio = ingresosViewModel.nTipoCambio,
                Ident_018_TipoPago = ingresosViewModel.nIdent_018_TipoPago,
                Ident_CuentasBancarias = ingresosViewModel.nIdent_CuentasBancarias,
                Ident_002_TipoMoneda = ingresosViewModel.nIdent_002_TipoMoneda,
                Importe = ingresosViewModel.nImporte,
                NumeroOperacion = ingresosViewModel.sNumeroOperacion,
                Fecha = ingresosViewModel.dFechaIngreso,
            };
            if (ingresosViewModel.nIdent_002_TipoMoneda == 24)
            {
                if (ingresosDetalleModel.Ident_002_TipoMoneda == ingresosViewModel.nIdent_002_TipoMoneda)
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte;
                }
                else
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte * ingresosViewModel.nTipoCambio;
                }
            }
            else
            {
                if (ingresosDetalleModel.Ident_002_TipoMoneda == ingresosViewModel.nIdent_002_TipoMoneda)
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte;
                }
                else
                {
                    ingresosDetalleModel.ImporteConTC = ingresosViewModel.nImporte / ingresosViewModel.nTipoCambio;
                }
            }
            ingresosDetalleModel.Ident_IngresosDetalle = await _cajaService.IngresosDetalle_Insert(ingresosDetalleModel, loginResult);
            await _cajaService.IngresosActualizarTotal(ingresosViewModel.nIdent_Ingresos);
            return RedirectToAction("IngresoVer", "Caja", new { nIdent_Ingresos = ingresosViewModel.nIdent_Ingresos });
        }
        [HttpGet]
        public async Task<IActionResult> EgresosIndex()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            var egresosIndexFilterDTO = new EgresosIndexFilterDTO
            {
                dFechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                dFechaHasta = DateTime.Now
            };

            var egresosIndexTablaDTO = await _cajaService.EgresosIndex(egresosIndexFilterDTO);

            var egresosIndexViewModel = new EgresosIndexViewModel
            {
                dFechaDesde = egresosIndexFilterDTO.dFechaDesde,
                dFechaHasta = egresosIndexFilterDTO.dFechaHasta,
                lEgresosTabla = egresosIndexTablaDTO,
                nTotalSoles = egresosIndexTablaDTO
                .Where(x => x.nIdent_002_TipoMoneda == 24)
                .Sum(x => x.nImporte),
                nTotalDolares = egresosIndexTablaDTO
                .Where(x => x.nIdent_002_TipoMoneda == 23)
                .Sum(x => x.nImporte),
            };

            egresosIndexViewModel.lTipoEgreso = await _tablasService.ListarTipoEgreso();

            return View(egresosIndexViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EgresosIndex(EgresosIndexViewModel egresosIndexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            EgresosIndexFilterDTO egresosIndexFilterDTO = new EgresosIndexFilterDTO { 
                dFechaDesde = egresosIndexViewModel.dFechaDesde,
                dFechaHasta = egresosIndexViewModel.dFechaHasta,
                nIdent_022_TipoEgresos = egresosIndexViewModel.nIdent_022_TipoEgresos,
                sBuscar = egresosIndexViewModel.sBuscar,
            };

            var egresosIndexTablaDTo = await _cajaService.EgresosIndex(egresosIndexFilterDTO);
            egresosIndexViewModel.lEgresosTabla = egresosIndexTablaDTo;

            egresosIndexViewModel.nTotalDolares = egresosIndexTablaDTo
                .Where(x => x.nIdent_002_TipoMoneda == 23)
                .Sum(x => x.nImporte);
            egresosIndexViewModel.nTotalSoles = egresosIndexTablaDTo
                .Where(x => x.nIdent_002_TipoMoneda == 24)
                .Sum(x => x.nImporte);
            egresosIndexViewModel.lTipoEgreso = await _tablasService.ListarTipoEgreso();

            return View(egresosIndexViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> EgresoNuevo()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            EgresosViewModel egresosViewModel = new EgresosViewModel();
            egresosViewModel.lTipoEgreso = await _tablasService.ListarTipoEgreso();
            egresosViewModel.dFechaEgreso = DateTime.Now;
            egresosViewModel.lBancos = await _tablasService.ListarBancos();
            egresosViewModel.lTipoMonedas = await _tablasService.ListarTipoMoneda();
            egresosViewModel.lTipoPagos = await _tablasService.ListarTipoPago();
            return View(egresosViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EgresoNuevo(EgresosViewModel egresosViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            EgresosDTO egresoDTO = new EgresosDTO
            {
                nIdent_022_TipoEgresos = egresosViewModel.nIdent_022_TipoEgresos,
                nIdent_Persona = egresosViewModel.nIdent_Persona,
                sObservacion = egresosViewModel.sObservacion,
                dFechaEgreso = egresosViewModel.dFechaEgreso,
                nIdent_002_TipoMoneda = egresosViewModel.nIdent_002_TipoMoneda,
                nUsuarioCreacion = loginResult.IdentUsuario,
            };

            egresosViewModel.nIdent_Egresos = await _cajaService.EgresosInsertar(egresoDTO);

            EgresosDetalleDTO egresosDetalleDTO = new EgresosDetalleDTO 
            {
                nIdent_Egresos = egresosViewModel.nIdent_Egresos,
                nIdent_018_TipoPago = egresosViewModel.nIdent_018_TipoPago,
                nIdent_019_Banco = egresosViewModel.nIdent_019_Banco,
                nIdent_002_TipoMoneda = egresosViewModel.nIdent_002_TipoMoneda,
                dFecha = egresosViewModel.dFechaEgreso,
                nImporte = egresosViewModel.nImporte,
                sNumeroOperacion = egresosViewModel.sNumeroOperacion,
                nUsuarioCreacion = loginResult.IdentUsuario,
            };

            egresosDetalleDTO.nIdent_EgresosDetalle = await _cajaService.EgresosDetalleInsertar(egresosDetalleDTO);

            return RedirectToAction("EgresoVer", "Caja", new { nIdent_Egresos = egresosViewModel.nIdent_Egresos });
        }
        [HttpGet]
        public async Task<IActionResult> EgresoVer(int nIdent_Egresos, string sMensaje)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            if (nIdent_Egresos > 0)
            {
                HttpContext.Session.SetInt32("nIdent_Egresos", nIdent_Egresos);
            }
            else
            {
                nIdent_Egresos = (int)HttpContext.Session.GetInt32("nIdent_Egresos");
            }

            EgresosViewModel egresosViewModel = await _cajaService.EgresosSelect(nIdent_Egresos);
            egresosViewModel.lTipoEgreso = await _tablasService.ListarTipoEgreso();
            egresosViewModel.lBancos = await _tablasService.ListarBancos();
            egresosViewModel.lTipoMonedas = await _tablasService.ListarTipoMoneda();
            egresosViewModel.lTipoPagos = await _tablasService.ListarTipoPago();

            egresosViewModel.lEgresosDetallesList = await _cajaService.EgresosDetalle_List(egresosViewModel.nIdent_Egresos);

            return View(egresosViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EgresoVer(EgresosViewModel egresosViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            EgresosDetalleDTO egresosDetalleDTO = new EgresosDetalleDTO
            {
                nIdent_Egresos = egresosViewModel.nIdent_Egresos,
                nIdent_018_TipoPago = egresosViewModel.nIdent_018_TipoPago,
                nIdent_019_Banco = egresosViewModel.nIdent_019_Banco,
                nIdent_002_TipoMoneda = egresosViewModel.nIdent_002_TipoMoneda,
                dFecha = egresosViewModel.dFechaEgreso,
                nImporte = egresosViewModel.nImporte,
                sNumeroOperacion = egresosViewModel.sNumeroOperacion,
                nUsuarioCreacion = loginResult.IdentUsuario,
            };

            egresosDetalleDTO.nIdent_EgresosDetalle = await _cajaService.EgresosDetalleInsertar(egresosDetalleDTO);

            return RedirectToAction("EgresoVer", "Caja", new { nIdent_Egresos = egresosViewModel.nIdent_Egresos });
        }

        [HttpGet]
        public async Task<IActionResult> GetCuentasBancarias(int Ident_019_banco)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var cuentasBancariasList = await _cajaService.CuentasBancariasXBanco(Ident_019_banco);
            return Json(cuentasBancariasList);
        }
        [HttpGet]
        public async Task<IActionResult> GetManzanas(int programaId)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var manzanas = await _cajaService.ManzanaCbxListar(programaId);
            return Json(manzanas);
        }
        [HttpGet]
        public async Task<IActionResult> GetLotes(int manzanaId)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var lotes = await _cajaService.LoteCbxListar(manzanaId);
            return Json(lotes);
        }
        [HttpGet]
        public async Task<IActionResult> ImprimirRecibo(int nIdent_Ingreso, int nIdent_Persona)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            // 1. Obtener los datos desde el SP
            var recibo = await _contratosService.ImprimirRecibo(nIdent_Ingreso,nIdent_Persona);

            if (recibo == null)
                return NotFound("No se encontró el recibo.");

            // 2. Instanciar la clase generadora y generar el archivo
            var generador = new ReciboGenerador(_webHostEnvironment.WebRootPath,recibo.nIdent_021_TipoIngresos);
            byte[] documento = await generador.GenerarReciboAsync(recibo);

            // 3. Retornar el archivo como descarga
            return File(documento,
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        $"Recibo_{recibo.NumeroRecibo}.docx");
        }
        [HttpGet]
        public async Task<IActionResult> Eliminar_IngresosDetalle(int Ident_IngresosDetalle)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            await _cajaService.IngresosDetalle_Delete(Ident_IngresosDetalle, loginResult);

            //await _cajaService.IngresosActualizarTotal(ingresosViewModel.nIdent_Ingresos);
            //await _cajaService.Ingresos_ValidarImportes(Ident_IngresosDetalle, 135);
            return RedirectToAction("Cuotas", "Contratos", new { mensaje = "se eliminó el detalle" });
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar_EgresosDetalle(int nIdent_EgresosDetalle)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            EgresosDetalleDTO egresosDetalleDTO = new EgresosDetalleDTO { 
                nIdent_EgresosDetalle = nIdent_EgresosDetalle,
                nUsuarioModificacion = loginResult.IdentUsuario,
            };

            await _cajaService.EgresosDetalle_Delete(egresosDetalleDTO);

            return RedirectToAction("EgresoVer", "Caja");
        }
    }
}
