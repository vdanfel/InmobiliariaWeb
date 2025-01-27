using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace InmobiliariaWeb.Controllers
{
    public class ContratosController:Controller
    {
        private readonly IContratosService _contratosService;
        public ContratosController(IContratosService contratosService)
        {
            _contratosService = contratosService;
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
        public async Task<IActionResult> Crear(string numeroSeparacion)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                CrearViewModel crearViewModel = new CrearViewModel();
                crearViewModel.ProgramasCbxLists = await _contratosService.ProgramaCbxListar();
                crearViewModel.FechaContrato = DateTime.Now;
                crearViewModel.FechaCuotaInicial = DateTime.Now;
                if (numeroSeparacion != null || numeroSeparacion != "")
                {
                    crearViewModel.NumeroSeparacion = numeroSeparacion;
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
        public async Task<IActionResult> Crear(CrearViewModel crearViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var ident_Contrato = await _contratosService.CrearContrato(crearViewModel, loginResult);

                if (ident_Contrato == 0)
                {
                    crearViewModel.Mensaje = "No se pudo crear el contrato";
                    return View(crearViewModel);
                }

                if (crearViewModel.NumeroSeparacion != null)
                {
                    await _contratosService.InsertarClientesxSeparacion(crearViewModel.NumeroSeparacion, ident_Contrato, loginResult);
                }
                crearViewModel.Mensaje = "Se registró el Contrato";
                HttpContext.Session.SetInt32("Ident_Contrato", ident_Contrato);
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
                ActualizarViewModel actualizarViewModel = new ActualizarViewModel();
                actualizarViewModel = await _contratosService.ContratoxIdentContrato(Ident_Contratos);
                actualizarViewModel.Mensaje = Mensaje;
                HttpContext.Session.SetInt32("Ident_017_TipoContrato", actualizarViewModel.Ident_017_TipoContrato);
                var ident_Kardex = await _contratosService.IdentKardexXIdentContrato(Ident_Contratos);
                if (ident_Kardex > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Kardex", ident_Kardex);
                }
                HttpContext.Session.SetString("NumeroSerie", actualizarViewModel.NumeroSerie);
                HttpContext.Session.SetInt32("EstadoImpresion", actualizarViewModel.EstadoImpresion ? 1 : 0);
                return View(actualizarViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(ActualizarViewModel actualizarViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var Ident_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contrato");
                await _contratosService.ActualizarContrato(Ident_Contratos, actualizarViewModel, loginResult);
                actualizarViewModel.Mensaje = "Se actualizó con éxito";
                return RedirectToAction("Actualizar", "Contratos", new { Ident_Contratos = Ident_Contratos, Mensaje = actualizarViewModel.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        public IActionResult Anular(int Ident_Contrato, string Mensaje)
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cliente(int Ident_Contratos, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ClienteViewModel clienteViewModel = new ClienteViewModel();
                clienteViewModel.EstadoImpresion = HttpContext.Session.GetInt32("EstadoImpresion") == 1;
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
                    clienteViewModel.Mensaje = Mensaje;
                }
                clienteViewModel.Ident_Contratos = Ident_Contratos;
                clienteViewModel.Clientes = await _contratosService.ClientesxContrato(clienteViewModel);
                clienteViewModel.Numero_Contrato = HttpContext.Session.GetString("NumeroSerie");
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
                KardexViewModel kardexViewModel = new KardexViewModel();
                kardexViewModel.CuotasListas = await _contratosService.ListarCuotas(ident_Kardex);
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
        public IActionResult Cuotas()
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
        [HttpGet]
        [Authorize]
        public IActionResult Moras()
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
        [Authorize]
        [HttpGet]
        public IActionResult Imprimir(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                int Ident_017_TipoContrato = Int32.Parse(HttpContext.Session.GetInt32("Ident_017_TipoContrato").ToString());
                if (Ident_017_TipoContrato == 113)
                {
                    return RedirectToAction("Ventas", "Contratos", new { Ident_Contratos = Ident_Contratos });
                }
                else
                {
                    return RedirectToAction("Transferencias", "Contratos", new { Ident_Contratos = Ident_Contratos });
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ventas(int Ident_Contratos)
        {
            Ventas ventas = new Ventas();
            ventas = await _contratosService.FormatoContratoVentas(Ident_Contratos);
            return View(ventas);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Transferencias(int Ident_Contratos)
        {
            
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ContratoImpresion(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ImpresionContrato impresionContrato = new ImpresionContrato();
                impresionContrato = await _contratosService.ImprimirContrato(Ident_Contratos);

                return View(impresionContrato);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
           
        }
        [Authorize]
        public async Task<IActionResult> KardexCrear(int Ident_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ActualizarViewModel actualizarViewModel = new ActualizarViewModel();
                actualizarViewModel = await _contratosService.ContratoxIdentContrato(Ident_Contratos);
                var ident_Kardex = 0;
                ident_Kardex = await _contratosService.CrearKardex(Ident_Contratos, loginResult);
                if (ident_Kardex > 0)
                {
                    HttpContext.Session.SetInt32("Ident_Kardex", ident_Kardex);
                    if (actualizarViewModel.CuotaFinal > 0)
                    {
                        DateTime fechaCuota = actualizarViewModel.FechaCuotaInicial;
                        int diaOriginal = fechaCuota.Day;
                        for (int i = 1; i < actualizarViewModel.CantidadCuotas; i++)
                        {
                            // Crea la cuota con la fecha actualizada
                            await _contratosService.CrearCuotas(ident_Kardex, i, fechaCuota, actualizarViewModel.CuotasIniciales, loginResult);
                            // Aumenta el mes de la fecha de la cuota
                            fechaCuota = fechaCuota.AddMonths(1);
                            // Si la fecha resultante tiene menos días que el día original, ajusta al último día del mes
                            if (fechaCuota.Day < diaOriginal)
                            {
                                fechaCuota = new DateTime(fechaCuota.Year, fechaCuota.Month, DateTime.DaysInMonth(fechaCuota.Year, fechaCuota.Month));
                            }
                            else
                            {
                                // Mantén el mismo día del mes que el día original
                                fechaCuota = new DateTime(fechaCuota.Year, fechaCuota.Month, diaOriginal);
                            }
                        }
                        await _contratosService.CrearCuotas(ident_Kardex, actualizarViewModel.CantidadCuotas, fechaCuota, actualizarViewModel.CuotaFinal, loginResult);
                    }
                    else
                    {
                        DateTime fechaCuota = actualizarViewModel.FechaCuotaInicial;
                        int diaOriginal = fechaCuota.Day;
                        for (int i = 1; i <= actualizarViewModel.CantidadCuotas; i++)
                        {
                            // Crea la cuota con la fecha actualizada
                            await _contratosService.CrearCuotas(ident_Kardex, i, fechaCuota, actualizarViewModel.CuotasIniciales, loginResult);
                            // Aumenta el mes de la fecha de la cuota
                            fechaCuota = fechaCuota.AddMonths(1);
                            // Si la fecha resultante tiene menos días que el día original, ajusta al último día del mes
                            if (fechaCuota.Day < diaOriginal)
                            {
                                fechaCuota = new DateTime(fechaCuota.Year, fechaCuota.Month, DateTime.DaysInMonth(fechaCuota.Year, fechaCuota.Month));
                            }
                            else
                            {
                                // Mantén el mismo día del mes que el día original
                                fechaCuota = new DateTime(fechaCuota.Year, fechaCuota.Month, diaOriginal);
                            }
                        }
                    }

                }
                return RedirectToAction("Kardex", "Contratos", new { Ident_Contratos = Ident_Contratos });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
    }
}
