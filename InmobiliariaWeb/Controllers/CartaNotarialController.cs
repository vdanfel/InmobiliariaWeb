using BusinessLogic.Interface.CartaNotarial;
using BusinessLogic.Interface.Lote;
using BusinessLogic.Interface.Manzana;
using BusinessLogic.Interface.Programa;
using Domain.CartaNotarial;
using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Tablas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    [Authorize]
    public class CartaNotarialController : Controller
    {
        private readonly IContratosService _contratosService;
        private readonly ICartaNotarialService _cartaNotarialService;
        private readonly ICartaNotarialBL _cartaNotarialBL;
        private readonly IManzanaBL _manzanaBL;
        private readonly ILoteBL _loteBL;
        private readonly IProgramaBL _programaBL;
        public CartaNotarialController(IContratosService contratosService, ICartaNotarialService cartaNotarialService, ICartaNotarialBL cartaNotarialBL, IManzanaBL manzanaBL, ILoteBL loteBL, IProgramaBL programaBL)
        {
            _contratosService = contratosService;
            _cartaNotarialService = cartaNotarialService;
            _cartaNotarialBL = cartaNotarialBL;
            _manzanaBL = manzanaBL;
            _loteBL = loteBL;
            _programaBL = programaBL;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            CartaNotarialViewDTO cartaNotarialViewDTO = new CartaNotarialViewDTO();
            CartaNotarialRequestDTO cartaNotarialRequestDTO = new CartaNotarialRequestDTO();
            cartaNotarialViewDTO.lCartaNotarialList = (await _cartaNotarialBL.CartaNotarialBandeja(cartaNotarialRequestDTO)).ToList();
            cartaNotarialViewDTO.lProgramas = (await _programaBL.ProgramaConCartaNotarial()).ToList();
            return View(cartaNotarialViewDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CartaNotarialViewDTO cartaNotarialViewDTO)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            CartaNotarialRequestDTO cartaNotarialRequestDTO = new CartaNotarialRequestDTO { 
                nIdent_Programa = cartaNotarialViewDTO.nIdent_Programa,
                nIdent_Manzana = cartaNotarialViewDTO.nIdent_Manzana,
                nIdent_Lote = cartaNotarialViewDTO.nIdent_Lote,
                sBuscar = cartaNotarialViewDTO.sBuscar,
            };

            cartaNotarialViewDTO.lCartaNotarialList = (await _cartaNotarialBL.CartaNotarialBandeja(cartaNotarialRequestDTO)).ToList();
            cartaNotarialViewDTO.lProgramas = (await _programaBL.ProgramaConCartaNotarial()).ToList();

            return View(cartaNotarialViewDTO);
        }
        [HttpGet]
        public async Task<IActionResult> ManzanasConCartaNotarial(int programaId)
        {
            var manzanas = await _manzanaBL.ManzanaConCartaNotarialOpciones(programaId);
            return Json(manzanas);
        }
        [HttpGet]
        public async Task<IActionResult> ManzanasConContrato(int programaId)
        {
            var manzanas = await _manzanaBL.ManzanaConContratoOpciones(programaId);
            return Json(manzanas);
        }
        [HttpGet]
        public async Task<IActionResult> LoteConCartaNotarial(int manzanaId)
        {
            var lotes = await _loteBL.LoteConCartaNotarialOpciones(manzanaId);
            return Json(lotes);
        }
        [HttpGet]
        public async Task<IActionResult> LoteConContrato(int manzanaId)
        {
            var lotes = await _loteBL.LoteConContratoOpciones(manzanaId);
            return Json(lotes);
        }
        [HttpGet]
        public async Task<IActionResult> GetContratoPorLote(int loteId)
        {
            var contrato = await _cartaNotarialService.ObtenerContratoPorLote(loteId);
            if (contrato == null)
                return NotFound("No se encontró un contrato asociado al lote seleccionado.");

            return Json(contrato);
        }
        [HttpGet]
        public async Task<IActionResult> GetClientesPorContrato(int contratoId)
        {
            var clientes = await _cartaNotarialService.ListarClientesPorContrato(contratoId);
            return Json(clientes);
        }
        [HttpGet]
        public async Task<IActionResult> CartaNotarial1Crear()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            CartaNotarial1ViewDTO cartaNotarial1ViewDTO = new CartaNotarial1ViewDTO();
            cartaNotarial1ViewDTO.lPrograma = (await _programaBL.ProgramaConContrato()).ToList();
            cartaNotarial1ViewDTO.dFechaCartaNotarial = DateTime.Now;
            return View(cartaNotarial1ViewDTO);
        }
        [HttpPost]
        public async Task<IActionResult> CartaNotarial1Crear(CartaNotarial1ViewDTO cartaNotarial1ViewDTO, int[] PersonasSeleccionadas)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            cartaNotarial1ViewDTO.nIdent_UsuarioCreacion = loginResult.IdentUsuario;

            CartaNotarialDTO cartaNotarialDTO = new CartaNotarialDTO
            {
                nIdent_Contratos = cartaNotarial1ViewDTO.nIdent_Contratos,
                dFechaCartaNotarial = cartaNotarial1ViewDTO.dFechaCartaNotarial,
                nIdent_027_TipoCartaNotarial = 1159,
                nIdent_CartaNotarialOrigen = null,
                nIdent_UsuarioCreacion = cartaNotarial1ViewDTO.nIdent_UsuarioCreacion
            };

            cartaNotarial1ViewDTO.nIdent_CartaNotarial = await _cartaNotarialService.CartaNotarialCreate(cartaNotarialDTO);

            CartaNotarialDetalleDTO cartaNotarialDetalleDTO = new CartaNotarialDetalleDTO
            {
                nIdent_CartaNotarial = cartaNotarial1ViewDTO.nIdent_CartaNotarial,
                nIdent_026_EstadoCartaNotarial = 1152,
                sObservacion = cartaNotarial1ViewDTO.sObservacion,
                nIdent_UsuarioCreacion = cartaNotarial1ViewDTO.nIdent_UsuarioCreacion
            };

            cartaNotarialDetalleDTO.nIdent_CartaNotarialDetalle = await _cartaNotarialService.CartaNotarialDetalleCreate(cartaNotarialDetalleDTO);

            foreach (var personaId in PersonasSeleccionadas)
            {
                var cartaNotarialPersona = new CartaNotarialPersonaDTO
                {
                    nIdent_CartaNotarial = cartaNotarial1ViewDTO.nIdent_CartaNotarial,
                    nIdent_Persona = personaId,
                    nIdent_UsuarioCreacion = cartaNotarial1ViewDTO.nIdent_UsuarioCreacion,
                };

                cartaNotarialPersona.nIdent_CartaNotarialPersona = await _cartaNotarialService.CartaNotarialPersonaCreate(cartaNotarialPersona);
            }

            HttpContext.Session.SetInt32("nIdent_CartaNotarial", cartaNotarial1ViewDTO.nIdent_CartaNotarial ?? 0);

            return RedirectToAction("CartaNotarial1Ver", "CartaNotarial", new { nIdent_CartaNotarial = cartaNotarial1ViewDTO.nIdent_CartaNotarial });
        }
        [HttpGet]
        public async Task<IActionResult> CartaNotarial1Ver(int nIdent_CartaNotarial)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            if (nIdent_CartaNotarial > 0)
            {
                HttpContext.Session.SetInt32("nIdent_CartaNotarial", nIdent_CartaNotarial);
            }
            else
            {
                nIdent_CartaNotarial = (int)HttpContext.Session.GetInt32("nIdent_CartaNotarial");
            }

            var cartaNotarial1ViewDTO = await _cartaNotarialBL.CartaNotarialSelect(nIdent_CartaNotarial);
            cartaNotarial1ViewDTO.lClientes = (await _cartaNotarialBL.CartaNotarialPersonaList(nIdent_CartaNotarial)).ToList();
            ViewData["ActiveTab"] = "CartaNotarial1Ver";
            return View(cartaNotarial1ViewDTO);
        }
        [HttpGet]
        public async Task<IActionResult> CartaNotarial1Formato(int nIdent_CartaNotarial)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            if (nIdent_CartaNotarial > 0)
            {
                HttpContext.Session.SetInt32("nIdent_CartaNotarial", nIdent_CartaNotarial);
            }
            else
            {
                nIdent_CartaNotarial = (int)HttpContext.Session.GetInt32("nIdent_CartaNotarial");
            }
            ViewData["ActiveTab"] = "CartaNotarial1Formato";
            return View();
        }

    }
}
