using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InmobiliariaWeb.Controllers
{
    public class ProgramaController : Controller
    {
        private readonly IProgramaService _programaService;
        private readonly IPersonaService _personaService;
        private readonly ITablasService _tablasService;

        public ProgramaController(IProgramaService programaService, IPersonaService personaService, ITablasService tablasService)
        {
            _programaService = programaService;
            _personaService = personaService;
            _tablasService = tablasService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ProgramaViewModel programaViewModel = new ProgramaViewModel();
                programaViewModel.ProgramaList = await _programaService.BandejaPrograma(programaViewModel.Buscar);
                return View(programaViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(ProgramaViewModel programaViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                programaViewModel.ProgramaList = await _programaService.BandejaPrograma(programaViewModel.Buscar);
                return View(programaViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Crear()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ProgramaCrearViewModel programaCrearViewModel = new ProgramaCrearViewModel();
                programaCrearViewModel.Manzanas = await _tablasService.ListarManzanas();
                programaCrearViewModel.TipoContratos = await _tablasService.ListarTipoContrato();
                return View(programaCrearViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear(ProgramaCrearViewModel programaCrearViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (programaCrearViewModel.ViewPrograma.NumeroPartida.IsNullOrEmpty())
                {
                    programaCrearViewModel.Mensaje = "El Número de Partida es Obligatorio";
                    programaCrearViewModel.Manzanas = await _tablasService.ListarManzanas();
                    programaCrearViewModel.TipoContratos = await _tablasService.ListarTipoContrato();
                    return View(programaCrearViewModel);
                }

                if (programaCrearViewModel.ViewPrograma.AreaTotal < programaCrearViewModel.ViewPrograma.AreaLotizada)
                {
                    programaCrearViewModel.Mensaje = "El Área Total no puede ser menor que el Área Lotizada";
                    programaCrearViewModel.Manzanas = await _tablasService.ListarManzanas();
                    programaCrearViewModel.TipoContratos = await _tablasService.ListarTipoContrato();
                    return View(programaCrearViewModel);
                }
                if (programaCrearViewModel.ViewPrograma.AreaTotal == 0 || programaCrearViewModel.ViewPrograma.AreaLotizada == 0)
                {
                    programaCrearViewModel.Mensaje = "El Área Total y el Área Lotizada no pueden estar en 0";
                    programaCrearViewModel.Manzanas = await _tablasService.ListarManzanas();
                    programaCrearViewModel.TipoContratos = await _tablasService.ListarTipoContrato();
                    return View(programaCrearViewModel);
                }
                programaCrearViewModel.ViewPrograma.IdentPrograma = await _programaService.RegistrarPrograma(programaCrearViewModel.ViewPrograma, loginResult);
                ViewPrograma viewprograma = new ViewPrograma();
                viewprograma = programaCrearViewModel.ViewPrograma;
                if (viewprograma.IdentPrograma > 0)
                {
                    var mensaje = await _programaService.RegistrarManzanas(viewprograma, loginResult);
                    if (mensaje == "ok")
                    {
                        viewprograma.Mensaje = "Se registró con éxito";
                    }
                    else
                    {
                        viewprograma.Mensaje = "Se registró el Programa pero no se registraron los lotes";
                    }
                    HttpContext.Session.SetInt32("IdentPrograma", viewprograma.IdentPrograma);
                    return RedirectToAction("ProgramaActualizar", "Programa", new { IdentPrograma = viewprograma.IdentPrograma, Mensaje = viewprograma.Mensaje });
                }
                else
                {
                    programaCrearViewModel.ViewPrograma.Mensaje = "Problemas al intentar registrar Programa";
                    programaCrearViewModel.Manzanas = await _tablasService.ListarManzanas();
                    programaCrearViewModel.TipoContratos = await _tablasService.ListarTipoContrato();
                    return View(programaCrearViewModel.ViewPrograma);
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuscarPersonas(string buscar)
        {
            try
            {
                var personas = await _personaService.PersonaBandeja(buscar);
                return Json(personas);
            }
            catch (Exception ex)
            {
                // Manejar errores según sea necesario
                return Json(new { error = ex.Message });
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ValidarManzanaInicial(int identPrograma, int manzanaInicial, int cantidadManzanas)
        {
            var mensaje = await _programaService.ValidarManzanaInicial(identPrograma, manzanaInicial, cantidadManzanas);
            return Json(new { Mensaje = mensaje });
        }
        [Authorize]
        public async Task<IActionResult> AnularPrograma(int identPrograma)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var mensaje = await _programaService.AnularPrograma(identPrograma);
                if (mensaje == "OK")
                {
                    mensaje = "Se anuló con éxito el Programa";
                }
                else
                {
                    mensaje = "No se pudo anular el Programa";
                }
                return RedirectToAction("Index", "Programa", new { IdentPrograma = identPrograma, Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ActualizarCantidadLotes(int identManzana, int nuevaCantidadLotes)
        {
            // Llamar al servicio para actualizar la cantidad de lotes
            var mensaje = await _programaService.ActualizarCantidadLotes(identManzana, nuevaCantidadLotes);
            return Json(new { mensaje });
        }
        [Authorize]
        public async Task<IActionResult> ListarManzanaJson(int identPrograma)
        {
            // Llamar al servicio para obtener la lista actualizada de manzanas
            var viewManzanas = await _programaService.ListarManzanasPrograma(identPrograma);
            // Devolver la vista parcial con los datos actualizados
            return Json(viewManzanas);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProgramaActualizar(int IdentPrograma, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (IdentPrograma == 0)
                {
                    IdentPrograma = (int)HttpContext.Session.GetInt32("IdentPrograma");
                }
                else
                {
                    HttpContext.Session.SetInt32("IdentPrograma", IdentPrograma);
                }
                var viewPrograma = await _programaService.BuscarProgramaIdentPrograma(IdentPrograma);
                if (Mensaje != null)
                {
                    viewPrograma.Mensaje = Mensaje;
                }
                HttpContext.Session.SetString("NombrePrograma", viewPrograma.NombrePrograma);
                viewPrograma.TipoPropietario = await _tablasService.ListarTipoPropietario();
                viewPrograma.manzanas = await _tablasService.ListarManzanas();
                viewPrograma.TipoContratos = await _tablasService.ListarTipoContrato();
                return View(viewPrograma);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProgramaActualizar(ViewPrograma viewPrograma)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var Mensaje = "";
                if (ModelState.IsValid)
                {

                    if (viewPrograma.AreaTotal < viewPrograma.AreaLotizada)
                    {
                        Mensaje = "El Area Total no puede ser menor que el Area Lotizada";
                        viewPrograma.manzanas = await _tablasService.ListarManzanas();
                        return View(viewPrograma);
                    }
                    if (viewPrograma.AreaTotal == 0 || viewPrograma.AreaLotizada == 0)
                    {
                        viewPrograma.Mensaje = "El Área Total y el Área Lotizada no pueden estar en 0";
                        viewPrograma.manzanas = await _tablasService.ListarManzanas();
                        return View(viewPrograma);
                    }
                    else
                    {
                        Mensaje = await _programaService.ActualizarPrograma(viewPrograma, loginResult);
                        var mensajemanzana = await _programaService.ValidarManzanaInicial(viewPrograma.IdentPrograma, viewPrograma.ManzanaInicial, viewPrograma.CantidadManzanas);
                        if (mensajemanzana == "OK")
                        {
                            mensajemanzana = await _programaService.AnularManzanasList(viewPrograma.IdentPrograma, loginResult.IdentUsuario);
                            mensajemanzana = await _programaService.RegistrarManzanas(viewPrograma, loginResult);
                        }
                        if (mensajemanzana != "ER")
                        {
                            Mensaje = "Se actualizó el Programa y las Manzanas";
                        }
                    }
                }
                else
                {
                    Mensaje = "Complete los datos obligatorios";
                }
                viewPrograma = await _programaService.BuscarProgramaIdentPrograma(viewPrograma.IdentPrograma);
                viewPrograma.TipoPropietario = await _tablasService.ListarTipoPropietario();
                viewPrograma.manzanas = await _tablasService.ListarManzanas();
                viewPrograma.TipoContratos = await _tablasService.ListarTipoContrato();
                viewPrograma.Mensaje = Mensaje;
                return View(viewPrograma);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Propietario(string mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                int IdentPrograma = (int)HttpContext.Session.GetInt32("IdentPrograma");
                var viewPropietario = new ViewPropietario();
                viewPropietario.IdentPrograma = IdentPrograma;
                viewPropietario.PropietarioList = await _programaService.ListarPropietario(IdentPrograma);
                viewPropietario.TipoPropietario = await _tablasService.ListarTipoPropietario();
                viewPropietario.NombrePrograma = HttpContext.Session.GetString("NombrePrograma");
                if (mensaje != null)
                {
                    viewPropietario.Mensaje = mensaje;
                }
                return View(viewPropietario);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Propietario(ViewPropietario viewPropietario)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (viewPropietario.IdentPersona == 0)
                {
                    viewPropietario.Mensaje = "Debe seleccionar una Persona";
                    return RedirectToAction("Propietario", "Programa", new { Mensaje = viewPropietario.Mensaje });
                }
                else
                {
                    if (viewPropietario.NumeroPartida == null && viewPropietario.Ident011TipoPropietario == 87)
                    {
                        viewPropietario.Mensaje = "Numero de partida es obligatorio para Apoderado";
                        return RedirectToAction("Propietario", "Programa", new { Mensaje = viewPropietario.Mensaje });
                    }
                    else
                    {
                        if (viewPropietario.NumeroPartida == null)
                        {
                            viewPropietario.NumeroPartida = "";
                        }
                        var identProgramaPropietario = await _programaService.RegistrarPropietario(viewPropietario, loginResult);
                        if (identProgramaPropietario > 0)
                        {
                            viewPropietario.Mensaje = "Se registró Propietario con Éxito";
                        }
                        else
                        {
                            viewPropietario.Mensaje = "No se pudo registrar Propietario";
                        }
                        return RedirectToAction("Propietario", "Programa", new { Mensaje = viewPropietario.Mensaje });
                    }
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manzana(string mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                int IdentPrograma = (int)HttpContext.Session.GetInt32("IdentPrograma");
                ViewManzana viewManzana = new ViewManzana();
                viewManzana.ManzanaList = await _programaService.ListarManzanasPrograma(IdentPrograma);
                viewManzana.NombrePrograma = HttpContext.Session.GetString("NombrePrograma");

                return View(viewManzana);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Manzana(int identManzana, int nuevaCantidadLotes)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                HttpContext.Session.SetInt32("IdentManzana", identManzana);
                // Llamar al servicio para actualizar la cantidad de lotes
                var mensaje = await _programaService.ActualizarCantidadLotes(identManzana, nuevaCantidadLotes);
                if (mensaje == "OK")
                {
                    await _programaService.RegistrarLote(identManzana, nuevaCantidadLotes, loginResult, 0);
                    mensaje = "Se actualizó con Éxito la cantidad de Lotes";
                }
                else
                {
                    mensaje = "Error al actualizar los lotes";
                }
                //return View("Manzana", new { mensaje = mensaje });
                return RedirectToAction("Manzana", "Programa", new { mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [Authorize]
        public async Task<IActionResult> PropietarioEliminar(int IdentProgramaPropietario)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var mensaje = await _programaService.AnularPropietario(IdentProgramaPropietario, loginResult.IdentUsuario);
                if (mensaje == "OK")
                {
                    mensaje = "Se anuló correctamente";
                }
                return RedirectToAction("Propietario", "Programa", new { Mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult>Lote (int IdentManzana,string Letra, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (IdentManzana == 0)
                {
                    IdentManzana = (int)HttpContext.Session.GetInt32("IdentManzana");
                }
                ViewLote viewLote = new ViewLote();
                viewLote.TipoLote = await _tablasService.listarTipoLote();
                viewLote.LotesList = await _programaService.ListarLotes(IdentManzana);
                viewLote.NombrePrograma = HttpContext.Session.GetString("NombrePrograma");
                if (Letra != null)
                {
                    HttpContext.Session.SetString("LetraManzana", Letra);
                    viewLote.LetraManzana = Letra;
                }
                else
                {
                    viewLote.LetraManzana = HttpContext.Session.GetString("LetraManzana");
                }

                if (Mensaje != null)
                {
                    viewLote.Mensaje = Mensaje;
                }
                HttpContext.Session.SetInt32("IdentManzana", IdentManzana);
                return View(viewLote);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [Authorize]
        public async Task<IActionResult> Lado(int IdentLote, int Ident010TipoLote)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                await DatosProManLot(0, 0, IdentLote);
                var NumeroLote = HttpContext.Session.GetInt32("NumeroLote");
                if (Ident010TipoLote == 84)
                {
                    return RedirectToAction("LadoRegular", "Programa", new { identLote = IdentLote, numeroLote = NumeroLote });
                }
                else if (Ident010TipoLote == 85)
                {
                    return RedirectToAction("LadoEspecial", "Programa", new { identLote = IdentLote, numeroLote = NumeroLote });
                }
                else
                {
                    return RedirectToAction("Lote", "Programa", new { Mensaje = "Debe escoger un Tipo de Lote" });
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LadoRegular(int Identlote, string Mensaje, int NumeroLote)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ViewLadoregular viewLadoregular = await _programaService.LadoRegularSelect(Identlote);
                viewLadoregular.Ident_Lote = Identlote;
                if (Mensaje != null)
                {
                    viewLadoregular.Mensaje = Mensaje;
                }
                viewLadoregular.Ident_010_TipoLote = 84;
                viewLadoregular.NombrePrograma = HttpContext.Session.GetString("NombrePrograma");
                viewLadoregular.LetraManzana = HttpContext.Session.GetString("LetraManzana");
                if (NumeroLote != 0)
                {
                    HttpContext.Session.SetInt32("NumeroLote", NumeroLote);
                }
                else
                {
                    NumeroLote = (int)HttpContext.Session.GetInt32("NumeroLote");
                }
                viewLadoregular.NumeroLote = NumeroLote;
                viewLadoregular.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                return View(viewLadoregular);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LadoRegular(ViewLadoregular viewLadoregular)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (viewLadoregular.Area == 0 || viewLadoregular.PrecioM2 == 0)
                {
                    viewLadoregular.Mensaje = "El Area y el Precio del Metro cuadrado no pueden ser 0";
                    viewLadoregular.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                    return View(viewLadoregular);
                }
                else
                {
                    viewLadoregular.PrecioTotal = viewLadoregular.Area * viewLadoregular.PrecioM2;
                    var mensaje = await _programaService.LadoRegularRegistrar(viewLadoregular, loginResult);
                    if (mensaje == "ok")
                    {
                        if (viewLadoregular.FlagCheked == true && viewLadoregular.Ident_012_EstadoLote == 88)
                        {
                            viewLadoregular.Ident_012_EstadoLote = 89;
                        }
                        else if (viewLadoregular.FlagCheked == false && viewLadoregular.Ident_012_EstadoLote == 89)
                        {
                            viewLadoregular.Ident_012_EstadoLote = 88;
                        }
                        await _programaService.LoteActualizar(viewLadoregular.Ident_Lote, viewLadoregular.Ident_010_TipoLote, viewLadoregular.PrecioM2, viewLadoregular.Area, viewLadoregular.PrecioTotal,
                            loginResult, viewLadoregular.Ident_012_EstadoLote, viewLadoregular.Ident_014_UbicacionLote, viewLadoregular.Flag_ReservadoPropietarpio);

                        viewLadoregular.Mensaje = "Se registraron los lados con Éxito";
                    }
                    else
                    {
                        viewLadoregular.Mensaje = "Error al intentar registrar los lados";
                        viewLadoregular.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                        return View(viewLadoregular);
                    }
                    return RedirectToAction("LadoRegular", "Programa", new { IdentLote = viewLadoregular.Ident_Lote, Mensaje = viewLadoregular.Mensaje });
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LadoEspecial(int Identlote, string Mensaje, int NumeroLote)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ViewLadoEspecial viewLadoEspecial = await _programaService.LadoEspecialSelect(Identlote);
                viewLadoEspecial.tipoLados = await _tablasService.ListarLado();
                viewLadoEspecial.Ident_Lote = Identlote;
                if (Mensaje != null)
                {
                    viewLadoEspecial.Mensaje = Mensaje;
                }
                viewLadoEspecial.Ident_010_TipoLote = 85;
                viewLadoEspecial.NombrePrograma = HttpContext.Session.GetString("NombrePrograma");
                viewLadoEspecial.LetraManzana = HttpContext.Session.GetString("LetraManzana");
                if (NumeroLote != 0)
                {
                    HttpContext.Session.SetInt32("NumeroLote", NumeroLote);
                }
                else
                {
                    NumeroLote = (int)HttpContext.Session.GetInt32("NumeroLote");
                }
                viewLadoEspecial.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                return View(viewLadoEspecial);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LadoEspecial(ViewLadoEspecial viewLadoEspecial)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (viewLadoEspecial.Area == 0 || viewLadoEspecial.PrecioM2 == 0)
                {
                    viewLadoEspecial.Mensaje = "El Area y el Precio del Metro cuadrado no pueden ser 0";
                    viewLadoEspecial.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                    return View(viewLadoEspecial);
                }
                else
                {
                    if (viewLadoEspecial.L1 == 0 || viewLadoEspecial.L2 == 0 || viewLadoEspecial.L3 == 0 || viewLadoEspecial.L4 == 0 || viewLadoEspecial.L5 == 0)
                    {
                        viewLadoEspecial.Mensaje = "Lote especial debe tener mas de 4 lados";
                        viewLadoEspecial.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                        return View(viewLadoEspecial);
                    }
                    else
                    {
                        viewLadoEspecial.PrecioTotal = viewLadoEspecial.Area * viewLadoEspecial.PrecioM2;
                        var mensaje = await _programaService.LadoEspecialRegistrar(viewLadoEspecial, loginResult);

                        if (mensaje == "ok")
                        {
                            if (viewLadoEspecial.FlagCheked == true && viewLadoEspecial.Ident_012_EstadoLote == 88)
                            {
                                viewLadoEspecial.Ident_012_EstadoLote = 89;
                            }
                            else if (viewLadoEspecial.FlagCheked == false && viewLadoEspecial.Ident_012_EstadoLote == 89)
                            {
                                viewLadoEspecial.Ident_012_EstadoLote = 88;
                            }
                            await _programaService.LoteActualizar(viewLadoEspecial.Ident_Lote, viewLadoEspecial.Ident_010_TipoLote, viewLadoEspecial.PrecioM2, viewLadoEspecial.Area, viewLadoEspecial.PrecioTotal,
                                loginResult, viewLadoEspecial.Ident_012_EstadoLote, viewLadoEspecial.Ident_014_UbicacionLote, viewLadoEspecial.Flag_ReservadoPropietarpio);
                            viewLadoEspecial.Mensaje = "Se registraron los lados con Éxito";
                        }
                        else
                        {
                            viewLadoEspecial.Mensaje = "Error al intentar registrar los lados";
                            viewLadoEspecial.TipoUbicacionLotes = await _tablasService.ListarTipoUbicacionlote();
                            return View(viewLadoEspecial);
                        }
                        return RedirectToAction("LadoEspecial", "Programa", new { IdentLote = viewLadoEspecial.Ident_Lote, Mensaje = viewLadoEspecial.Mensaje });
                    }
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReporteLotes(int Ident_Programa,int tipoReporte)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                HttpContext.Session.SetInt32("IdentPrograma", Ident_Programa);
                var reporteProgramasEstado = await _programaService.ReporteProgramasxEstado(Ident_Programa, tipoReporte);
                return View(reporteProgramasEstado);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        public async Task DatosProManLot(int Ident_Programa, int Ident_Manzana,int Ident_Lote)
        {
            var promanlot = await _programaService.DatosProManLot(Ident_Programa, Ident_Manzana, Ident_Lote);
            HttpContext.Session.SetInt32("IdentPrograma", promanlot.Ident_Programa);
            HttpContext.Session.SetString("NombrePrograma", promanlot.Nombre_Programa ?? "");
            HttpContext.Session.SetInt32("IdentManzana", promanlot.Ident_Manzana);
            HttpContext.Session.SetString("LetraManzana", promanlot.Letra_Manzana ?? "");
            HttpContext.Session.SetInt32("IdentLote", promanlot.Ident_Lote);
            HttpContext.Session.SetInt32("NumeroLote", promanlot.Lote);
        }
    }
}
