using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.CartaNotarial;
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
        public CartaNotarialController(IContratosService contratosService, ICartaNotarialService cartaNotarialService)
        {
            _contratosService = contratosService;
            _cartaNotarialService = cartaNotarialService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }

            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetManzanas(int programaId)
        {
            var manzanas = await _contratosService.ManzanaCbxListar(programaId);
            return Json(manzanas);
        }
        [HttpGet]
        public async Task<IActionResult> GetLotes(int manzanaId)
        {
            var lotes = await _contratosService.LoteCbxListar(manzanaId);
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
            cartaNotarial1ViewDTO.lPrograma = await _contratosService.ProgramaCbxListar();
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
                    nIdent_UsuarioCreacion = cartaNotarial1ViewDTO.nIdent_UsuarioCreacion
                };

                cartaNotarialPersona.nIdent_CartaNotarialPersona = await _cartaNotarialService.CartaNotarialPersonaCreate(cartaNotarialPersona);
            }

            return RedirectToAction("CartaNotarial1Ver", "CartaNotarial");
        }
        [HttpGet]
        public async Task<IActionResult> CartaNotarial1Ver()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            ViewData["ActiveTab"] = "CartaNotarial1Ver";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CartaNotarial1Formato()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            ViewData["ActiveTab"] = "CartaNotarial1Formato";
            return View();
        }
    }
}
