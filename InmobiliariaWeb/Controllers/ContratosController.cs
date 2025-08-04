using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Servicios;
using InmobiliariaWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace InmobiliariaWeb.Controllers
{
    public class ContratosController:Controller
    {
        private readonly IContratosService _contratosService;
        private readonly ITablasService _tablasService;
        private readonly ICajaService _cajaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //_webHostEnvironment

        public ContratosController(IContratosService contratosService,ITablasService tablasService, ICajaService cajaService, IWebHostEnvironment webHostEnvironment)
        {
            _contratosService = contratosService;
            _tablasService = tablasService;
            _cajaService = cajaService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                IndexViewModel indexViewModel = new IndexViewModel();
                indexViewModel.PaginaActual = 1;
                indexViewModel.GrupoActual = 1;
                indexViewModel.Correlativo = 0;
                indexViewModel.Ident_Programa = 0;
                indexViewModel.Ident_Manzana = 0;
                indexViewModel.ContratosLists = await _contratosService.BandejaContratos(indexViewModel);
                indexViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();
                return View(indexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(IndexViewModel indexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                indexViewModel.ContratosLists = await _contratosService.BandejaContratos(indexViewModel);
                indexViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();
                indexViewModel.NumeroGrupos = (int)Math.Ceiling((double)indexViewModel.NumeroPaginas / 10);
                indexViewModel.GrupoActual = (int)Math.Ceiling((double)indexViewModel.PaginaActual / 10);
                return View(indexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLoteDetalle(int ident_Lote)
        {
            var lotes = await _contratosService.LoteDetalle(ident_Lote);
            return Json(lotes);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Crear(string numeroSeparacion, string mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ViewModels.Contratos.CrearViewModel crearViewModel = new ViewModels.Contratos.CrearViewModel();
                ContratosModel contratosModel = new ContratosModel();
                crearViewModel.ContratosModels = contratosModel;
                crearViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();
                crearViewModel.ContratosModels.FechaContrato = DateTime.Now;
                crearViewModel.ContratosModels.FechaCuotaInicial = DateTime.Now;
                if (numeroSeparacion != null || numeroSeparacion != "")
                {
                    crearViewModel.NumeroSeparacion = numeroSeparacion;
                }
                if (mensaje != "")
                {
                    crearViewModel.Mensaje = mensaje;
                }
                return View(crearViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear(ViewModels.Contratos.CrearViewModel crearViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var ident_Contrato = 0;
                if (crearViewModel.ContratosModels.SaldoAPagar == 0)
                {
                    crearViewModel.ContratosModels.CantidadCuotas = 0;
                    crearViewModel.ContratosModels.CuotaInicial = 0;
                    crearViewModel.ContratosModels.CuotaFinal = 0;
                    crearViewModel.ContratosModels.DiaCuota = 0;
                    ident_Contrato = await _contratosService.CrearContrato(crearViewModel, loginResult);
                }
                else
                {
                    if (crearViewModel.ContratosModels.CantidadCuotas > 0 && crearViewModel.ContratosModels.CuotaInicial >= 0 && crearViewModel.ContratosModels.DiaCuota > 0)
                    {
                        ident_Contrato = await _contratosService.CrearContrato(crearViewModel, loginResult);
                    }
                    else
                    {
                        crearViewModel.Mensaje = "Debe seleccionar los campos obligatorios de manera correcta";
                        return RedirectToAction("Crear", "Contratos", new { numeroSeparacion = crearViewModel.NumeroSeparacion, Mensaje = crearViewModel.Mensaje });
                    }
                }
                if (ident_Contrato == 0)
                {
                    crearViewModel.Mensaje = "No se pudo crear el contrato";
                    return RedirectToAction("Crear", "Contratos", new { numeroSeparacion = crearViewModel.NumeroSeparacion, Mensaje = crearViewModel.Mensaje });
                }
                else
                {
                    crearViewModel.ContratosModels.Ident_Contratos = ident_Contrato;
                    crearViewModel.Mensaje = "Se registró el Contrato";
                    HttpContext.Session.SetInt32("Ident_Contrato", ident_Contrato);

                }

                if (crearViewModel.NumeroSeparacion != null)
                {
                    await _contratosService.InsertarClientesxSeparacion(crearViewModel.NumeroSeparacion, ident_Contrato, loginResult);
                }
                
                return RedirectToAction("Actualizar", "Contratos", new { Ident_Contratos = ident_Contrato, Mensaje = crearViewModel.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Actualizar(int Ident_Contratos, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                ViewModels.Contratos.ActualizarViewModel actualizarViewModel = new ViewModels.Contratos.ActualizarViewModel();
                ProgramaModel programaModel = new ProgramaModel();
                actualizarViewModel.ProgramaModels = programaModel;
                ContratosModel contratosModel = new ContratosModel();
                actualizarViewModel.ContratosModels = contratosModel;
                actualizarViewModel = await _contratosService.ContratoxIdentContrato(Ident_Contratos);
                actualizarViewModel.Mensaje = Mensaje;
                HttpContext.Session.SetInt32("Ident_017_TipoContrato", actualizarViewModel.ProgramaModels.Ident_017_TipoContrato);
                var ident_Kardex = await _contratosService.IdentKardexXIdentContrato(Ident_Contratos);
                if (ident_Kardex > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Kardex", ident_Kardex);
                }
                else
                {
                    HttpContext.Session.Remove("Ident_Kardex");
                }
                HttpContext.Session.SetString("NumeroSerie", actualizarViewModel.NumeroSerie);
                HttpContext.Session.SetInt32("EstadoImpresion", actualizarViewModel.ContratosModels.EstadoImpresion ? 1 : 0);
                actualizarViewModel.ContratosModels.Ident_Contratos = Ident_Contratos;
                if (actualizarViewModel.ContratosModels.SaldoAPagar == 0)
                {
                    HttpContext.Session.SetInt32("Saldo", 0);
                }
                return View(actualizarViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(ViewModels.Contratos.ActualizarViewModel actualizarViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                if (actualizarViewModel.ContratosModels.SaldoAPagar == 0)
                {
                    HttpContext.Session.SetInt32("Saldo", 0);
                }
                await _contratosService.ActualizarContrato(Ident_Contratos, actualizarViewModel, loginResult);
                actualizarViewModel.Mensaje = "Se actualizó con éxito";
                return RedirectToAction("Actualizar", "Contratos", new { Ident_Contratos = Ident_Contratos, Mensaje = actualizarViewModel.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Anular(int Ident_Contratos, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                AnularViewModel anularViewModel = new AnularViewModel();
                anularViewModel.Actualizar = await _contratosService.ContratoxIdentContrato(Ident_Contratos);
                anularViewModel.Clientes = await _contratosService.ClientesxContrato(Ident_Contratos);
                anularViewModel.Actualizar.ContratosModels.Ident_Contratos = Ident_Contratos;
                return View(anularViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Anular(AnularViewModel anularViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var mensaje = await _contratosService.AnularContrato(anularViewModel, loginResult);
                return RedirectToAction("Index", "Contratos",new { Mensaje = mensaje});
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cliente(int Ident_Contratos, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ClienteViewModel clienteViewModel = new ClienteViewModel();
                
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                
                clienteViewModel.Ident_Contratos = Ident_Contratos;
                clienteViewModel.Clientes = await _contratosService.ClientesxContrato(clienteViewModel.Ident_Contratos);
                if (Mensaje != null)
                {
                    clienteViewModel.Mensaje = Mensaje;
                }
                clienteViewModel.Numero_Contrato = HttpContext.Session.GetString("NumeroSerie");
                var ident_Kardex = await _contratosService.IdentKardexXIdentContrato(Ident_Contratos);
                if (ident_Kardex > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Kardex", ident_Kardex);
                }
                else
                {
                    HttpContext.Session.Remove("Ident_Kardex");
                }
                clienteViewModel.EstadoImpresion = HttpContext.Session.GetInt32("EstadoImpresion") == 1;
                return View(clienteViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cliente(ClienteViewModel clienteViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (clienteViewModel.Ident_Persona > 0)
                {
                    clienteViewModel.Mensaje = await _contratosService.ClienteInsertar(clienteViewModel, loginResult);
                }
                else
                {
                    clienteViewModel.Mensaje = "debe seleccionar un cliente";
                }
                return RedirectToAction("Cliente", "Contratos", new { Ident_Contratos = clienteViewModel.Ident_Contratos, Mensaje = clienteViewModel.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [Authorize]
        public async Task<IActionResult> EliminarCliente(int Ident_Contratos, int Ident_ContratosPersonas)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var Mensaje = await _contratosService.ClienteEliminar(Ident_ContratosPersonas, loginResult);
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                return RedirectToAction("Cliente", "Contratos", new { Ident_Contratos = Ident_Contratos, Mensaje = Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> kardex(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                var ident_Kardex = await _contratosService.IdentKardexXIdentContrato(Ident_Contratos);
                if (ident_Kardex > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Kardex", ident_Kardex);
                }
                else
                {
                    HttpContext.Session.Remove("Ident_Kardex");
                }
                KardexViewModel kardexViewModel = new KardexViewModel();
                kardexViewModel.Ident_Kardex = ident_Kardex;
                var cuotasListas = await _contratosService.ListarCuotas(ident_Kardex, loginResult);
                kardexViewModel = await _contratosService.DatosKardex(ident_Kardex);
                kardexViewModel.CuotasListas = cuotasListas;
                kardexViewModel.Numero_Contrato = HttpContext.Session.GetString("NumeroSerie");
                return View(kardexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDatosxSeparacion(int numeroSeparacion)
        {
            var datosxSeparacion = await _contratosService.ObtenerxSeparacion(numeroSeparacion);
            return Json(datosxSeparacion);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cuotas(int Ident_Cuota,string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Cuota == 0)
                {
                    Ident_Cuota = (int)HttpContext.Session.GetInt32("Ident_Cuota");
                }
                else
                {
                    HttpContext.Session.SetInt32("Ident_Cuota", Ident_Cuota);
                }
                ViewBag.NumeroSerie = HttpContext.Session.GetString("NumeroSerie");
                
                Cuotas cuotas = new Cuotas();
                cuotas = await _contratosService.CuotasxIdentCuotas(Ident_Cuota);
                cuotas.TipoPagos = await _tablasService.ListarTipoPago();
                cuotas.Ident_Cuotas = Ident_Cuota;
                if (Mensaje != null)
                {
                    ViewBag.Mensaje = Mensaje;
                }
                cuotas.Ident_Kardex = (int)HttpContext.Session.GetInt32("Ident_Kardex");
                cuotas.Bancos = await _tablasService.ListarBancos();
                cuotas.TipoCuentaBancos = await _tablasService.ListarTipoCuentaBanco();
                cuotas.TipoMonedas = await _tablasService.ListarTipoMoneda();
                int Ident_Ingresos = await _cajaService.Obtener_Ident_Ingresos(135, cuotas.Ident_Cuotas);
                cuotas.ingresosDetallesLists = await _cajaService.IngresosDetalle_List(Ident_Ingresos);
                cuotas.ImporteTotalPagado = await _cajaService.IngresosDetalle_ImporteTotal(Ident_Ingresos);
                cuotas.SaldoAPagar = cuotas.ImporteCuota - cuotas.ImporteTotalPagado;
                cuotas.Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                return View(cuotas);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cuotas(Cuotas cuotas)
        {
            string mensaje = null;
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                int Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                int Ident_Ingresos = 0;
                IngresosModel ingresosModel = new IngresosModel();
                IngresosDetalleModel ingresosDetalleModel = new IngresosDetalleModel();
                ingresosModel = await _contratosService.IngresosCabecera(Ident_Contratos);
                ingresosModel.Ident_Origen = cuotas.Ident_Cuotas;
                ingresosModel.Ident_021_TipoIngresos = 135;
                ingresosModel.FechaPago = (DateTime)cuotas.FechaPagoRealizado;
                ingresosModel.Ident_002_TipoMoneda = 23;
                ingresosModel.ImporteTotal = cuotas.ImporteTotalPagado + cuotas.ImporteCuotasDolares;
                ingresosModel.Ident_015_EstadoPago = 109;
                if (ingresosModel.ImporteTotal == cuotas.ImporteCuota)
                {
                    ingresosModel.Ident_015_EstadoPago = 110;
                }
                Ident_Ingresos = await _cajaService.Obtener_Ident_Ingresos(135, cuotas.Ident_Cuotas);
                if (Ident_Ingresos == 0)
                {
                    Ident_Ingresos = await _cajaService.Ingresos_Insert(ingresosModel, loginResult);
                }
                else
                {
                    ingresosModel.Ident_Ingresos = Ident_Ingresos;
                    await _cajaService.Ingresos_Update(ingresosModel, loginResult);
                }
                if (Ident_Ingresos > 0)
                {
                    ingresosDetalleModel.Ident_Ingresos = Ident_Ingresos;
                    ingresosDetalleModel.TipoCambio = cuotas.TipoCambio;
                    ingresosDetalleModel.Ident_018_TipoPago = (int)cuotas.Ident_018_TipoPago;
                    ingresosDetalleModel.Ident_CuentasBancarias = cuotas.Ident_CuentasBancarias;
                    ingresosDetalleModel.Ident_002_TipoMoneda = cuotas.Ident_002_TipoMoneda;
                    ingresosDetalleModel.Importe = (decimal)cuotas.ImporteCuotasDolares;
                    ingresosDetalleModel.NumeroOperacion = cuotas.NumeroOperacion;
                    ingresosDetalleModel.ImporteConTC = (decimal)cuotas.ImporteCuotaPagado;
                    ingresosDetalleModel.Fecha = (DateTime)cuotas.FechaPagoRealizado;
                    int Ident_IngresosDetalle = await _cajaService.IngresosDetalle_Insert(ingresosDetalleModel, loginResult);
                    if (Ident_IngresosDetalle > 0)
                    {
                        if (cuotas.ImporteCuota == ingresosModel.ImporteTotal)
                        {
                            cuotas.ImporteCuotaPagado = ingresosModel.ImporteTotal;
                            mensaje = await _contratosService.CuotasActualizar(cuotas, loginResult);
                            if (mensaje == "")
                            {
                                if (cuotas.FechaPago > cuotas.FechaPagoRealizado)
                                {
                                    await _contratosService.MorasEliminar(cuotas.Ident_Cuotas);
                                }
                                mensaje = "Cuota Registrada Satisfactoriamente";
                            }
                        }
                        else 
                        {
                            mensaje = "Detalle registrado con exito";
                        }
                        await _contratosService.RecalculoMoras(cuotas.Ident_Kardex);
                    }
                }
                
                return RedirectToAction("Cuotas", "Contratos", new { Ident_Cuota = cuotas.Ident_Cuotas,Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Moras(int Ident_Cuotas, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ViewBag.NumeroSerie = HttpContext.Session.GetString("NumeroSerie");
                if (Ident_Cuotas == 0)
                {
                    Ident_Cuotas = (int)HttpContext.Session.GetInt32("Ident_Cuota");
                }
                else
                {
                    HttpContext.Session.SetInt32("Ident_Cuota", Ident_Cuotas);
                }

                Moras moras = new Moras();
                int Ident_Moras = await _contratosService.MoraExiste(Ident_Cuotas);
                if (Ident_Moras == 0)
                { 
                    Ident_Moras = await _contratosService.InsertarMoras(Ident_Cuotas, loginResult);
                }
                moras.Ident_Moras = Ident_Moras;
                moras = await _contratosService.ObtenerDatosMora(Ident_Moras);
                moras.TipoPagos = await _tablasService.ListarTipoPago();
                moras.Bancos = await _tablasService.ListarBancos();
                moras.TipoMonedas = await _tablasService.ListarTipoMoneda();
                int Ident_Ingresos = await _cajaService.Obtener_Ident_Ingresos(136, moras.Ident_Moras);
                moras.ingresosDetallesLists = await _cajaService.IngresosDetalle_List(Ident_Ingresos);
                moras.ImporteTotalPagado = await _cajaService.IngresosDetalle_ImporteTotal(Ident_Ingresos);
                moras.SaldoAPagar = (decimal)moras.NuevoMontoMora - moras.ImporteTotalPagado;
                if (Mensaje != null)
                {
                    ViewBag.Mensaje = Mensaje;
                }
                int desactivarCampos = moras.ingresosDetallesLists.Count();
                ViewBag.DesactivarCampos = desactivarCampos;
                moras.Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                return View(moras);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Moras(Moras moras)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                string mensaje = "";
                int Ident_Ingresos = 0;
                int Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                if (moras.NuevoMontoMora < moras.ImporteMoras)
                {
                    await _contratosService.MorasActualizar(moras, loginResult);
                }
                if (moras.NuevoMontoMora == 0)
                {
                    mensaje = await _contratosService.MorasActualizar(moras, loginResult);
                    if (mensaje == "")
                    {
                        await _contratosService.RecalculoMoras(moras.Ident_Kardex);
                        mensaje = "Se registró el pago de Mora con éxito";
                    }
                }
                else
                {
                    IngresosModel ingresosModel = new IngresosModel();
                    IngresosDetalleModel ingresosDetalleModel = new IngresosDetalleModel();
                    ingresosModel = await _contratosService.IngresosCabecera(Ident_Contratos);
                    ingresosModel.Ident_Origen = moras.Ident_Moras;
                    ingresosModel.Ident_021_TipoIngresos = 136;
                    ingresosModel.FechaPago = (DateTime)moras.FechaPagoRealizado;
                    ingresosModel.Ident_002_TipoMoneda = 23;
                    ingresosModel.ImporteTotal = moras.ImporteTotalPagado + moras.ImporteMorasDolares;
                    ingresosModel.Ident_015_EstadoPago = 109;
                    if (ingresosModel.ImporteTotal == moras.NuevoMontoMora)
                    {
                        ingresosModel.Ident_015_EstadoPago = 110;
                    }
                    Ident_Ingresos = await _cajaService.Obtener_Ident_Ingresos(136, moras.Ident_Moras);
                    if (Ident_Ingresos == 0)
                    {
                        Ident_Ingresos = await _cajaService.Ingresos_Insert(ingresosModel, loginResult);
                    }
                    else
                    {
                        ingresosModel.Ident_Ingresos = Ident_Ingresos;
                        await _cajaService.Ingresos_Update(ingresosModel, loginResult);
                    }
                    if (Ident_Ingresos>0)
                    {
                        ingresosDetalleModel.Ident_Ingresos = Ident_Ingresos;
                        ingresosDetalleModel.TipoCambio = moras.TipoCambio;
                        ingresosDetalleModel.Ident_018_TipoPago = (int)moras.Ident_018_TipoPago;
                        ingresosDetalleModel.Ident_CuentasBancarias = moras.Ident_CuentasBancarias;
                        ingresosDetalleModel.Ident_002_TipoMoneda = moras.Ident_002_TipoMoneda;
                        ingresosDetalleModel.Importe = (decimal)moras.ImporteMorasDolares;
                        ingresosDetalleModel.NumeroOperacion = moras.NumeroOperacion;
                        ingresosDetalleModel.ImporteConTC = (decimal)moras.ImporteMorasPagado;
                        ingresosDetalleModel.Fecha = (DateTime)moras.FechaPagoRealizado;
                        int Ident_IngresosDetalle = await _cajaService.IngresosDetalle_Insert(ingresosDetalleModel, loginResult);
                        if (Ident_IngresosDetalle > 0)
                        {
                            if (moras.NuevoMontoMora == ingresosModel.ImporteTotal)
                            {
                                moras.ImporteMorasPagado = ingresosModel.ImporteTotal;
                                mensaje = await _contratosService.MorasActualizar(moras, loginResult);
                                if (mensaje == "")
                                {
                                    await _contratosService.RecalculoMoras(moras.Ident_Kardex);
                                    mensaje = "Se registró el pago de Mora con éxito";
                                }
                            }
                            else
                            {
                                mensaje = "Detalle registrado con exito";
                            }
                        }
                    }
                }
               
                return RedirectToAction("Moras", "Contratos", new { Ident_Cuotas = moras.Ident_Cuotas, Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MorasMasivo(string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ViewBag.NumeroSerie = HttpContext.Session.GetString("NumeroSerie");

                MorasMasivoViewModel morasMasivoViewModel = new MorasMasivoViewModel();
                morasMasivoViewModel.Ident_Kardex = (int)HttpContext.Session.GetInt32("Ident_Kardex");
                morasMasivoViewModel.Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                morasMasivoViewModel.ImporteMorasTotal = await _contratosService.MorasMasivo_Total(morasMasivoViewModel.Ident_Kardex);
                morasMasivoViewModel.DescuentoDirecto = 0;
                morasMasivoViewModel.DescuentoPorcentaje = 0;
                morasMasivoViewModel.NuevoMontoMora = 0;
                morasMasivoViewModel.TipoPagos = await _tablasService.ListarTipoPago();
                morasMasivoViewModel.Bancos = await _tablasService.ListarBancos();
                morasMasivoViewModel.TipoMonedas = await _tablasService.ListarTipoMoneda();
                morasMasivoViewModel.FechaPago = DateTime.Now;
                //int Ident_Ingresos = await _cajaService.Obtener_Ident_Ingresos(137, morasMasivoViewModel.Ident_Kardex);
                //morasMasivoViewModel.ingresosDetallesLists = await _cajaService.IngresosDetalle_List(Ident_Ingresos);
                morasMasivoViewModel.ingresosDetallesLists = new List<IngresosDetallesList>();
                if (Mensaje != null)
                {
                    ViewBag.Mensaje = Mensaje;
                }
                return View(morasMasivoViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MorasMasivo(MorasMasivoViewModel morasMasivoViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                string mensaje = "";
                var ingresosModel = await _contratosService.IngresosCabecera(morasMasivoViewModel.Ident_Contratos);
                ingresosModel.Ident_Origen = morasMasivoViewModel.Ident_Kardex;
                ingresosModel.Ident_021_TipoIngresos = 137;
                ingresosModel.FechaPago = (DateTime)morasMasivoViewModel.FechaPago;
                ingresosModel.Ident_002_TipoMoneda = 23;
                ingresosModel.ImporteTotal = morasMasivoViewModel.ImporteTotalPagado + morasMasivoViewModel.ImporteMorasDolares ?? 0;
                if (morasMasivoViewModel.SaldoAPagar == 0)
                {
                    ingresosModel.Ident_015_EstadoPago = 110;
                    var moraPagoDTO = new MorasMasivoPagoDTO
                    {
                        Ident_Kardex = morasMasivoViewModel.Ident_Kardex,
                        DescuentoPorcentaje = morasMasivoViewModel.DescuentoPorcentaje ?? 0,
                        DescuentoDirectoTotal = morasMasivoViewModel.DescuentoDirecto ?? 0
                    };
                    await _contratosService.MoraMasivoPago(moraPagoDTO, loginResult);
                }
                else
                {
                    ingresosModel.Ident_015_EstadoPago = 109;
                }
                var Ident_Ingresos = await _cajaService.Obtener_Ident_Ingresos(137, morasMasivoViewModel.Ident_Kardex);

                if (Ident_Ingresos == 0)
                {
                    Ident_Ingresos = await _cajaService.Ingresos_Insert(ingresosModel, loginResult);
                }
                else
                {
                    ingresosModel.Ident_Ingresos = Ident_Ingresos;
                    await _cajaService.Ingresos_Update(ingresosModel, loginResult);
                }
                if (Ident_Ingresos > 0)
                {
                    var ingresosDetalleModel = new IngresosDetalleModel();
                    ingresosDetalleModel.Ident_Ingresos = Ident_Ingresos;
                    ingresosDetalleModel.TipoCambio = morasMasivoViewModel.TipoCambio;
                    ingresosDetalleModel.Ident_018_TipoPago = (int)morasMasivoViewModel.Ident_018_TipoPago;
                    ingresosDetalleModel.Ident_CuentasBancarias = morasMasivoViewModel.Ident_CuentasBancarias;
                    ingresosDetalleModel.Ident_002_TipoMoneda = morasMasivoViewModel.Ident_002_TipoMoneda;
                    ingresosDetalleModel.Importe = (decimal)morasMasivoViewModel.ImporteMorasDolares;
                    ingresosDetalleModel.NumeroOperacion = morasMasivoViewModel.NumeroOperacion;
                    ingresosDetalleModel.ImporteConTC = (decimal)morasMasivoViewModel.ImporteMorasPagado;
                    ingresosDetalleModel.Fecha = (DateTime)morasMasivoViewModel.FechaPago;
                    int Ident_IngresosDetalle = await _cajaService.IngresosDetalle_Insert(ingresosDetalleModel, loginResult);
                    if (Ident_IngresosDetalle > 0)
                    {
                            mensaje = "Detalle registrado con exito";
                    }
                }
                return RedirectToAction("MorasMasivo", "Contratos", new { Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Imprimir(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                int Ident_017_TipoContrato = Int32.Parse(HttpContext.Session.GetInt32("Ident_017_TipoContrato").ToString());
                int? saldo = HttpContext.Session.GetInt32("Saldo");
                if (Ident_017_TipoContrato == 113)
                {
                    if (saldo.HasValue && saldo == 0)
                    {
                        // Si es tipo 113 y el saldo es 0, redirigir a VentasContado
                        return RedirectToAction("VentasContado", "Contratos", new { Ident_Contratos = Ident_Contratos });
                    }
                    else
                    {
                        // Si es tipo 113 y el saldo es distinto de 0, redirigir a Ventas
                        return RedirectToAction("Ventas", "Contratos", new { Ident_Contratos = Ident_Contratos });
                    }
                }
                else
                {
                    if (saldo.HasValue && saldo == 0)
                    {
                        // Si es tipo 114 y el saldo es 0, redirigir a TransferenciasContado
                        return RedirectToAction("TransferenciasContado", "Contratos", new { Ident_Contratos = Ident_Contratos });
                    }
                    else
                    {
                        // Si es tipo 114 y el saldo es distinto de 0, redirigir a Transferencias
                        return RedirectToAction("Transferencias", "Contratos", new { Ident_Contratos = Ident_Contratos });
                    }
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ventas(int Ident_Contratos, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                if (Mensaje != null)
                {
                    ViewBag.Mensaje = Mensaje;
                }
                Ventas ventas = new Ventas();
                if (!_contratosService.FormatoVenta_Existe(Ident_Contratos))
                {
                    await _contratosService.FormatoVentas_Insert(Ident_Contratos,loginResult);
                }
                ventas = await _contratosService.FormatoVentas_List(Ident_Contratos);
                ventas.Ident_Contrato = Ident_Contratos;
                return View(ventas);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Ventas(Ventas ventas)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                await _contratosService.FormatoVentas_Update(ventas, loginResult);
                int Ident_Contratos = ventas.Ident_Contrato;
                string ContratosFormato = $@"
                <div class='Titulo'>{ventas.Titulo}</div>
                <div class='Parrafo'>{ventas.ParrafoInicial}</div>
                <div class='Parrafo'>{ventas.Clausula1}</div>
                <div class='Parrafo'>{ventas.Clausula2}</div>
                <div class='Parrafo'>{ventas.Clausula3}</div>
                <div class='Parrafo'>{ventas.Clausula4}</div>
                <div class='Parrafo'>{ventas.Clausula5}</div>
                <div class='Parrafo'>{ventas.Clausula6}</div>
                <div class='Parrafo'>{ventas.Clausula7}</div>
                <div class='Parrafo'>                
                    <ul>
                        <li>{ventas.ClausulaAllanamiento}</li>
                        <li>{ventas.ClausulaDesalojo}</li>
                    </ul>
                </div>
                <div class='Parrafo'>{ventas.Clausula8}</div>
                <div class='Parrafo'>{ventas.Clausula9}</div>
                <div class='Parrafo'>{ventas.Clausula10}</div>
                <div class='Parrafo'>{ventas.Clausula11}</div>
                <div class='Parrafo'>{ventas.Clausula12}</div>
                <div class='Parrafo'>{ventas.Clausula13}</div>
                <div class='Parrafo'>{ventas.Clausula14}</div>
                <div class='Parrafo'>{ventas.Clausula15}</div>
                <div class='Parrafo'>{ventas.Clausula16}</div>
                <div class='Parrafo'>{ventas.Clausula17}</div>
                <div class='Parrafo'>{ventas.ClausulaAdicional}</div>
                <div class='Parrafo'>                
                    <ul>
                        <li>{ventas.TextoFrente}</li>
                        <li>{ventas.TextoDerecha}</li>
                        <li>{ventas.TextoIzquierda}</li>
                        <li>{ventas.TextoFondo}</li>
                    </ul>
                </div>
                <div class='Parrafo'>{ventas.FechaContrato}</div>";

                var involucrados = await _contratosService.ObtenerInvolucrados(Ident_Contratos);
                ventas.Involucrados = involucrados;

                // Dividir involucrados en vendedores/inmobiliaria y compradores
                var vendedoresInmobiliaria = ventas.Involucrados
                    .Where(i => i.TipoPersona == "VENDEDOR(A)" || i.TipoPersona == "INMOBILIARIA")
                    .OrderBy(i => i.TipoPersona == "INMOBILIARIA") // Prioriza los vendedores primero
                    .ToList();

                var compradores = ventas.Involucrados
                    .Where(i => i.TipoPersona == "COMPRADOR(A)")
                    .ToList();

                // Determinar el número máximo de filas
                int maxFilas = Math.Max(vendedoresInmobiliaria.Count, compradores.Count);

                // Iniciar el formato HTML de la tabla
                ContratosFormato += @"<table class='tabla-involucrados'>";

                // Obtiene las cabeceras
                var cabeceras = await _contratosService.ObtenerInvolucradosCabecera(Ident_Contratos);
                var partesCabecera = cabeceras.Split('|');

                // Asigna los valores separados
                var cabeceraIzquierda = partesCabecera.Length > 0 ? partesCabecera[0] : "";
                var cabeceraDerecha = partesCabecera.Length > 1 ? partesCabecera[1] : "";
                
                // Agregar la fila inicial con los valores de la cabecera
                ContratosFormato += $@"
                <tr>
                    <td>
                        <br/>
                        <div class='datos'>{cabeceraIzquierda}</div>
                    </td>
                    <td>
                        <br/>
                        <div class='datos'>{cabeceraDerecha}</div>
                    </td>
                </tr>";

                // Generar las filas
                for (int i = 0; i < maxFilas; i++)
                {
                    ContratosFormato += "<tr>";

                    // Vendedores/Inmobiliaria
                    if (i < vendedoresInmobiliaria.Count)
                    {
                        var involucrado = vendedoresInmobiliaria[i];
                        ContratosFormato += $@"
                        <td>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <div class='datos'>{involucrado.Separacion}</div>
                            <div class='datos'>{involucrado.NombreCompleto}</div>
                            <div class='datos'>{involucrado.TipoDocumento} {involucrado.NumeroDocumento}</div>";
                        if (!string.IsNullOrEmpty(involucrado.NumeroPartida))
                        {
                            ContratosFormato += $"<div class='datos'>{involucrado.NumeroPartida}</div>";
                        }
                        else
                        {
                            ContratosFormato += "<div class='datos'></div>";
                        }
                        ContratosFormato += "</td>";
                    }
                    else
                    {
                        // Celda vacía si no hay más vendedores/inmobiliaria
                        ContratosFormato += "<td></td>";
                    }

                    // Compradores
                    if (i < compradores.Count)
                    {
                        var involucrado = compradores[i];
                        ContratosFormato += $@"
                        <td>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <div class='datos'>{involucrado.Separacion}</div>
                            <div class='datos'>{involucrado.NombreCompleto}</div>
                            <div class='datos'>{involucrado.TipoDocumento} {involucrado.NumeroDocumento}</div>";
                        if (!string.IsNullOrEmpty(involucrado.NumeroPartida))
                        {
                            ContratosFormato += $"<div class='datos'>{involucrado.NumeroPartida}</div>";
                        }
                        else
                        {
                            ContratosFormato += "<div class='datos'></div>";
                        }
                        ContratosFormato += "</td>";
                    }
                    else
                    {
                        // Celda vacía si no hay más compradores
                        ContratosFormato += "<td></td>";
                    }

                    ContratosFormato += "</tr>";
                }

                // Cerrar la tabla
                ContratosFormato += "</table>";
                
                string mensaje = await _contratosService.RegistrarFormatoImpreso(Ident_Contratos, ContratosFormato, loginResult);
                
                if (string.IsNullOrEmpty(mensaje))
                {
                    HttpContext.Session.SetInt32("EstadoImpresion", 1);
                    mensaje = "Se registró el Formato de impresión";
                    await CrearKardexYCuotas(Ident_Contratos, loginResult);
                }
                return RedirectToAction("Cliente", "Contratos", new { Ident_Contratos = Ident_Contratos, Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> VentasContado(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                VentasContado ventasContado = new VentasContado();
                ventasContado = await _contratosService.FormatoContratoVentasContado(Ident_Contratos);
                ventasContado.Ident_Contrato = Ident_Contratos;
                return View(ventasContado);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VentasContado(VentasContado ventasContado)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                int Ident_Contratos = ventasContado.Ident_Contrato;
                string ContratosFormato = $@"
                <div class='Titulo'>{ventasContado.Titulo}</div>
                <div class='Parrafo'>{ventasContado.ParrafoInicial}</div>
                <div class='Parrafo'><b><u>PRIMERA</u>.- </b>{ventasContado.Clausula1}</div>
                <div class='Parrafo'><b><u>SEGUNDA</u>.- </b>{ventasContado.Clausula2}</div>
                <div class='Parrafo'><b><u>TERCERA</u>.- </b>{ventasContado.Clausula3I}</div>
                <div class='Parrafo'>                
                    <ul>
                        <li>{ventasContado.TextoFrente}</li>
                        <li>{ventasContado.TextoDerecha}</li>
                        <li>{ventasContado.TextoIzquierda}</li>
                        <li>{ventasContado.TextoFondo}</li>
                    </ul>
                </div>
                <div class='Parrafo'>{ventasContado.Clausula3F}</div>
                <div class='Parrafo'><b><u>CUARTA</u>.- </b>{ventasContado.Clausula4}</div>
                <div class='Parrafo'><b><u>QUINTA</u>.- </b>{ventasContado.Clausula5}</div>
                <div class='Parrafo'><b><u>SEXTA</u>.- </b>{ventasContado.Clausula6}</div>
                <div class='Parrafo'><b><u>SEPTIMA</u>.- </b>{ventasContado.Clausula7}</div>
                <div class='Parrafo'><b><u>OCTAVA</u>.- </b>{ventasContado.Clausula8}</div>
                <div class='Parrafo'><b><u>NOVENA</u>.- </b>{ventasContado.Clausula9}</div>
                <div class='Parrafo'><b><u>DECIMA</u>.- </b>{ventasContado.Clausula10}</div>
                <div class='Parrafo'><b><u>DECIMA PRIMERA</u>.- </b>{ventasContado.Clausula11}</div>
                <div class='Parrafo'>{ventasContado.FechaContrato}</div>";

                var involucrados = await _contratosService.ObtenerInvolucrados(Ident_Contratos);
                ventasContado.Involucrados = involucrados;

                // Dividir involucrados en vendedores/inmobiliaria y compradores
                var vendedoresInmobiliaria = ventasContado.Involucrados
                    .Where(i => i.TipoPersona == "VENDEDOR(A)" || i.TipoPersona == "INMOBILIARIA")
                    .OrderBy(i => i.TipoPersona == "INMOBILIARIA") // Prioriza los vendedores primero
                .ToList();

                var compradores = ventasContado.Involucrados
                    .Where(i => i.TipoPersona == "COMPRADOR(A)")
                    .ToList();

                // Determinar el número máximo de filas
                int maxFilas = Math.Max(vendedoresInmobiliaria.Count, compradores.Count);

                // Iniciar el formato HTML de la tabla
                ContratosFormato += @"<table class='tabla-involucrados'>";

                // Obtiene las cabeceras
                var cabeceras = await _contratosService.ObtenerInvolucradosCabecera(Ident_Contratos);
                var partesCabecera = cabeceras.Split('|');

                // Asigna los valores separados
                var cabeceraIzquierda = partesCabecera.Length > 0 ? partesCabecera[0] : "";
                var cabeceraDerecha = partesCabecera.Length > 1 ? partesCabecera[1] : "";

                // Agregar la fila inicial con los valores de la cabecera
                ContratosFormato += $@"
                <tr>
                    <td>
                        <br/>
                        <div class='datos'>{cabeceraIzquierda}</div>
                    </td>
                    <td>
                        <br/>
                        <div class='datos'>{cabeceraDerecha}</div>
                    </td>
                </tr>";

                // Generar las filas
                for (int i = 0; i < maxFilas; i++)
                {
                    ContratosFormato += "<tr>";

                    // Vendedores/Inmobiliaria
                    if (i < vendedoresInmobiliaria.Count)
                    {
                        var involucrado = vendedoresInmobiliaria[i];
                        ContratosFormato += $@"
                        <td>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <div class='datos'>{involucrado.Separacion}</div>
                            <div class='datos'>{involucrado.NombreCompleto}</div>
                            <div class='datos'>{involucrado.TipoDocumento} {involucrado.NumeroDocumento}</div>";
                        if (!string.IsNullOrEmpty(involucrado.NumeroPartida))
                        {
                            ContratosFormato += $"<div class='datos'>{involucrado.NumeroPartida}</div>";
                        }
                        else
                        {
                            ContratosFormato += "<div class='datos'></div>";
                        }
                        ContratosFormato += "</td>";
                    }
                    else
                    {
                        // Celda vacía si no hay más vendedores/inmobiliaria
                        ContratosFormato += "<td></td>";
                    }

                    // Compradores
                    if (i < compradores.Count)
                    {
                        var involucrado = compradores[i];
                        ContratosFormato += $@"
                        <td>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <div class='datos'>{involucrado.Separacion}</div>
                            <div class='datos'>{involucrado.NombreCompleto}</div>
                            <div class='datos'>{involucrado.TipoDocumento} {involucrado.NumeroDocumento}</div>";
                        if (!string.IsNullOrEmpty(involucrado.NumeroPartida))
                        {
                            ContratosFormato += $"<div class='datos'>{involucrado.NumeroPartida}</div>";
                        }
                        else
                        {
                            ContratosFormato += "<div class='datos'></div>";
                        }
                        ContratosFormato += "</td>";
                    }
                    else
                    {
                        // Celda vacía si no hay más compradores
                        ContratosFormato += "<td></td>";
                    }

                    ContratosFormato += "</tr>";
                }

                // Cerrar la tabla
                ContratosFormato += "</table>";
                /**/

                
                string mensaje = await _contratosService.RegistrarFormatoImpreso(Ident_Contratos, ContratosFormato, loginResult);

                if (string.IsNullOrEmpty(mensaje))
                {
                    HttpContext.Session.SetInt32("EstadoImpresion", 1);
                    mensaje = "Se registró el Formato de impresión";
                    await CrearKardexYCuotas(Ident_Contratos, loginResult);
                }
                return RedirectToAction("Cliente", "Contratos", new { Ident_Contratos = Ident_Contratos, Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Transferencias(int Ident_Contratos, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Contratos > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
                }
                else
                {
                    Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                }
                if (Mensaje != null)
                {
                    ViewBag.Mensaje = Mensaje;
                }
                Transferencias transferencias = new Transferencias();
                if (!_contratosService.FormatoTransferencia_Existe(Ident_Contratos))
                {
                    await _contratosService.FormatoTransferencias_Insert(Ident_Contratos, loginResult);
                }
                transferencias = await _contratosService.FormatoTransferencias_List(Ident_Contratos);
                transferencias.Ident_Contrato = Ident_Contratos;
                return View(transferencias);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Transferencias(Transferencias transferencias)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                await _contratosService.FormatoTransferencias_Update(transferencias, loginResult);
                int Ident_Contratos = transferencias.Ident_Contrato;
                string ContratosFormato = $@"
                <div class='Titulo'>{transferencias.Titulo}</div>
                <div class='Parrafo'>{transferencias.ParrafoInicial}</div>
                <div class='Parrafo'>{transferencias.Clausula1}</div>
                <div class='Parrafo'>{transferencias.Clausula2}</div>
                <div class='Parrafo'>{transferencias.Clausula3}</div>                
                <div class='Parrafo'>                
                    <ul>
                        <li>{transferencias.TextoFrente}</li>
                        <li>{transferencias.TextoDerecha}</li>
                        <li>{transferencias.TextoIzquierda}</li>
                        <li>{transferencias.TextoFondo}</li>
                    </ul>
                </div>
                <div class='Parrafo'>{transferencias.Clausula4}</div>
                <div class='Parrafo'>{transferencias.Clausula5}</div>
                <div class='Parrafo'>{transferencias.Clausula6}</div>
                <div class='Parrafo'>{transferencias.Clausula7}</div>
                <div class='Parrafo'>{transferencias.Clausula8}</div>
                <div class='Parrafo'>{transferencias.Clausula9}</div>
                <div class='Parrafo'>                
                    <ul>
                        <li>{transferencias.ClausulaAllanamiento}</li>
                        <li>{transferencias.ClausulaDesalojo}</li>
                    </ul>
                </div>
                <div class='Parrafo'>{transferencias.Clausula10}</div>
                <div class='Parrafo'>{transferencias.Clausula11}</div>
                <div class='Parrafo'>{transferencias.Clausula12}</div>
                <div class='Parrafo'>{transferencias.Clausula13}</div>
                <div class='Parrafo'>{transferencias.FechaContrato}</div>";

                var involucrados = await _contratosService.ObtenerInvolucrados(Ident_Contratos);
                transferencias.Involucrados = involucrados;

                // Dividir involucrados en vendedores/inmobiliaria y compradores
                var vendedoresInmobiliaria = transferencias.Involucrados
                    .Where(i => i.TipoPersona == "TRANSFERENTE" || i.TipoPersona == "INMOBILIARIA")
                    .OrderBy(i => i.TipoPersona == "INMOBILIARIA") // Prioriza los vendedores primero
                .ToList();

                var compradores = transferencias.Involucrados
                    .Where(i => i.TipoPersona == "ADQUIRIENTE")
                    .ToList();

                // Determinar el número máximo de filas
                int maxFilas = Math.Max(vendedoresInmobiliaria.Count, compradores.Count);

                // Iniciar el formato HTML de la tabla
                ContratosFormato += @"<table class='tabla-involucrados'>";

                // Obtiene las cabeceras
                var cabeceras = await _contratosService.ObtenerInvolucradosCabecera(Ident_Contratos);
                var partesCabecera = cabeceras.Split('|');

                // Asigna los valores separados
                var cabeceraIzquierda = partesCabecera.Length > 0 ? partesCabecera[0] : "";
                var cabeceraDerecha = partesCabecera.Length > 1 ? partesCabecera[1] : "";

                // Agregar la fila inicial con los valores de la cabecera
                ContratosFormato += $@"
                <tr>
                    <td>
                        <br/>
                        <div class='datos'>{cabeceraIzquierda}</div>
                    </td>
                    <td>
                        <br/>
                        <div class='datos'>{cabeceraDerecha}</div>
                    </td>
                </tr>";

                // Generar las filas
                for (int i = 0; i < maxFilas; i++)
                {
                    ContratosFormato += "<tr>";

                    // Vendedores/Inmobiliaria
                    if (i < vendedoresInmobiliaria.Count)
                    {
                        var involucrado = vendedoresInmobiliaria[i];
                        ContratosFormato += $@"
                        <td>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <div class='datos'>{involucrado.Separacion}</div>
                            <div class='datos'>{involucrado.NombreCompleto}</div>
                            <div class='datos'>{involucrado.TipoDocumento} {involucrado.NumeroDocumento}</div>";
                        if (!string.IsNullOrEmpty(involucrado.NumeroPartida))
                        {
                            ContratosFormato += $"<div class='datos'>{involucrado.NumeroPartida}</div>";
                        }
                        else
                        {
                            ContratosFormato += "<div class='datos'></div>";
                        }
                        ContratosFormato += "</td>";
                    }
                    else
                    {
                        // Celda vacía si no hay más vendedores/inmobiliaria
                        ContratosFormato += "<td></td>";
                    }

                    // Compradores
                    if (i < compradores.Count)
                    {
                        var involucrado = compradores[i];
                        ContratosFormato += $@"
                        <td>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <div class='datos'>{involucrado.Separacion}</div>
                            <div class='datos'>{involucrado.NombreCompleto}</div>
                            <div class='datos'>{involucrado.TipoDocumento} {involucrado.NumeroDocumento}</div>";
                        if (!string.IsNullOrEmpty(involucrado.NumeroPartida))
                        {
                            ContratosFormato += $"<div class='datos'>{involucrado.NumeroPartida}</div>";
                        }
                        else
                        {
                            ContratosFormato += "<div class='datos'></div>";
                        }
                        ContratosFormato += "</td>";
                    }
                    else
                    {
                        // Celda vacía si no hay más compradores
                        ContratosFormato += "<td></td>";
                    }

                    ContratosFormato += "</tr>";
                }

                // Cerrar la tabla
                ContratosFormato += "</table>";
                string mensaje = await _contratosService.RegistrarFormatoImpreso(Ident_Contratos, ContratosFormato, loginResult);

                if (string.IsNullOrEmpty(mensaje))
                {
                    HttpContext.Session.SetInt32("EstadoImpresion", 1);
                    mensaje = "Se registró el Formato de impresión";
                    await CrearKardexYCuotas(Ident_Contratos, loginResult);
                }
                return RedirectToAction("Cliente", "Contratos", new { Ident_Contratos = Ident_Contratos, Mensaje = mensaje });


            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TransferenciasContado(int Ident_Contado)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                TransferenciasContado transferenciasContado = new TransferenciasContado();
                transferenciasContado.Ident_Contratos = Ident_Contado;
                return View(transferenciasContado);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TransferenciasContado(TransferenciasContado transferenciasContado)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                return View();
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        private async Task CrearKardexYCuotas(int Ident_Contratos, LoginResult loginResult)
        {
            var actualizarViewModel = await _contratosService.ContratoxIdentContrato(Ident_Contratos);
            var ident_Kardex = await _contratosService.CrearKardex(actualizarViewModel, loginResult);
        }

        public async Task<IActionResult> DescargarPDF(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }

            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            if (Ident_Contratos > 0)
            {
                HttpContext.Session.SetInt32("Ident_Contrato", Ident_Contratos);
            }
            else
            {
                Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
            }

            string ContratosFormato = await _contratosService.ObtenerFormato(Ident_Contratos);

            if (string.IsNullOrEmpty(ContratosFormato))
            {
                return RedirectToAction("Cliente", "Contratos",
                    new { Ident_Contratos = Ident_Contratos, Mensaje = "No existe registro de este contrato" });
            }

            string fileName = "documento"+Ident_Contratos+".doc";

            string wordContent = $@"
            <html xmlns:o='urn:schemas-microsoft-com:office:office' 
                  xmlns:w='urn:schemas-microsoft-com:office:word'
                  xmlns='http://www.w3.org/TR/REC-html40'>
            <head>
                <meta charset='utf-8'/>
                <title>Exportar a Word</title>
                <xml>
                    <w:WordDocument>
                        <w:View>Print</w:View>
                        <w:Zoom>100</w:Zoom>
                        <w:DoNotOptimizeForBrowser/>
                    </w:WordDocument>
                </xml>
                <style>
                    body{{
                        margin-left:-10px
                        margin-right:-10px
                    }}
                    p {{
                        margin-bottom:5px;
                        font-size: 16px;
                        text-align: justify;
                        font-family: 'Arial Narrow';
                        line-height: 13px;
                    }}

                    .Titulo {{
                        font-weight: bold;
                        font-size: 21px;
                        text-align: center;
                        font-family: 'Arial Narrow';
                        margin-bottom:15px;
                    }}

                    .Parrafo {{
                        font-size: 16px;
                        text-align: justify;
                        font-family: 'Arial Narrow';
                        margin-top:10px;
                        margin-left:-10px
                        margin-right:-10px
                    }}

                    .espacio {{
                        margin: 20px 0;
                    }}

                    .datos {{
                        text-align: center;
                        font-size: 12px;
                        font-weight: bold;
                        font-family: 'Arial Narrow';
                    }}

                    .tabla-involucrados {{
                        width: 100%;
                        border-collapse: collapse;
                        border-spacing: 0;
                    }}

                    .tabla-involucrados td {{
                        border: none;
                        padding: 0;
                        font-family: 'Arial Narrow';
                    }}

                    b, strong {{
                        font-family: 'Arial Narrow';
                    }}

                    sup {{
                        font-family: 'Arial Narrow';
                        font-size: 0.8em;
                    }}
                </style>
            </head>
            <body>
                {ContratosFormato}
            </body>
            </html>";

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(wordContent);

            return File(byteArray, "application/msword", fileName);
        }
       
        
        [HttpPost]
        public async Task<IActionResult> FormatoVenta_Actualizar([FromBody] Ventas ventas)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                await _contratosService.FormatoVentas_Update(ventas, loginResult);
                return Json(new { mensaje = "Se actualizó con éxito" });
            }
            else
            {
                return Json(new { mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> FormatoTransferencia_Actualizar([FromBody] Transferencias transferencias)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                await _contratosService.FormatoTransferencias_Update(transferencias, loginResult);
                return Json(new { mensaje = "Se actualizó con éxito" });
            }
            else
            {
                return Json(new { mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Eliminar_IngresosDetalle_Cuota(int Ident_IngresosDetalle)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            await _cajaService.IngresosDetalle_Delete(Ident_IngresosDetalle, loginResult);
            await _cajaService.Ingresos_ValidarImportes(Ident_IngresosDetalle, 135);
            return RedirectToAction("Cuotas", "Contratos", new { mensaje = "se eliminó el detalle" });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Eliminar_IngresosDetalle_Mora(int Ident_IngresosDetalle)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            await _cajaService.IngresosDetalle_Delete(Ident_IngresosDetalle, loginResult);
            await _cajaService.Ingresos_ValidarImportes(Ident_IngresosDetalle, 136);
            return RedirectToAction("Moras", "Contratos", new { mensaje = "se eliminó el detalle" });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Eliminar_IngresosDetalle_MoraMasivo(int Ident_IngresosDetalle)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            await _cajaService.IngresosDetalle_Delete(Ident_IngresosDetalle, loginResult);
            await _cajaService.Ingresos_ValidarImportes(Ident_IngresosDetalle, 137);
            return RedirectToAction("Cuotas", "Contratos", new { mensaje = "se eliminó el detalle" });
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerPropietarios(int identContrato)
        {
            var propietarios = await _contratosService.ClientesxContrato(identContrato);
            var resultado = propietarios.Select(p => new {
                p.Ident_ContratosPersonas,
                p.NumeroDocumento,
                p.Nombre_Completo
            });
            return Json(resultado);
        }
        [HttpGet]
        public async Task<IActionResult> ImprimirRecibo(int Ident_021_TipoIngresos, int Ident_Origen, int Ident_ContratosPersonas)
        {
            // 1. Obtener los datos desde el SP
            var recibo = await _contratosService.ImprimirRecibo(Ident_021_TipoIngresos, Ident_Origen, Ident_ContratosPersonas);

            if (recibo == null)
                return NotFound("No se encontró el recibo.");

            // 2. Instanciar la clase generadora y generar el archivo
            var generador = new ReciboGenerador(_webHostEnvironment.WebRootPath);
            byte[] documento = await generador.GenerarReciboAsync(recibo);

            // 3. Retornar el archivo como descarga
            return File(documento,
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        $"Recibo_{recibo.NumeroRecibo}.docx");
        }
    }
}
