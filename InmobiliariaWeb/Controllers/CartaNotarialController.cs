using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.CartaNotarial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    [Authorize]
    public class CartaNotarialController:Controller
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
        public async Task<IActionResult> CartaNotarial1Crear()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            CartaNotarial1ViewDTO cartaNotarial1ViewDTO = new CartaNotarial1ViewDTO();
            cartaNotarial1ViewDTO.lPrograma = await _contratosService.ProgramaCbxListar();

            return View(cartaNotarial1ViewDTO);
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
    }
}
