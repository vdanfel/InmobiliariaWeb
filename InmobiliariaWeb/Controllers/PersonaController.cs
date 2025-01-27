using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Persona;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Persona;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Runtime.CompilerServices;

namespace InmobiliariaWeb.Controllers
{
    public class PersonaController:Controller
    {
        private readonly ITablasService _tablasService;
        private readonly IPersonaService _personaService;

        public PersonaController(ITablasService tablasService, IPersonaService personaService)
        {
            _tablasService = tablasService;
            _personaService = personaService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                PersonaViewModel personaViewModel = new PersonaViewModel();
                personaViewModel.PersonaList = await _personaService.PersonaBandeja(personaViewModel.Buscar);
                return View(personaViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(PersonaViewModel personaViewModel, string Buscar)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                personaViewModel.PersonaList = await _personaService.PersonaBandeja(Buscar);
                return View(personaViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Crear( string mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                PersonaCrearViewModel personaCrearViewModel = new PersonaCrearViewModel();
                personaCrearViewModel.TipoDocumento = await _tablasService.ListarTipoDocumento();
                personaCrearViewModel.TipoEstadoCivil = await _tablasService.ListarTipoEstadoCivil();
                personaCrearViewModel.ListDepartamento = await _tablasService.ListarDepartamento();
                personaCrearViewModel.TipoSexos = await _tablasService.ListarSexo();
                personaCrearViewModel.Paises = await _tablasService.ListarPaises();
                if (mensaje != null)
                {
                    personaCrearViewModel.Mensaje = mensaje;

                }
                personaCrearViewModel.FechaNacimiento = DateTime.Now;
                return View(personaCrearViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear(PersonaCrearViewModel personaCrearViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (personaCrearViewModel.FechaNacimiento < new DateTime(1900, 1, 1))
                {
                    personaCrearViewModel.FechaNacimiento = DateTime.Now;
                }
                int existe = await _personaService.PersonaExiste(personaCrearViewModel);

                if (existe == 1)
                {
                    personaCrearViewModel.Mensaje = "El documento ya existe";
                    return RedirectToAction("Crear", personaCrearViewModel);
                }
                else
                {
                    PersonaResult personaResult = new PersonaResult();
                    personaResult = await _personaService.PersonaRegistrar(personaCrearViewModel, loginResult);
                    return RedirectToAction("Actualizar", "Persona", new { identPersona = personaResult.IdentPersona, mensaje = personaResult.Mensaje });
                }
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Actualizar(int identPersona, string mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                PersonaList personaList = new PersonaList();
                personaList = await _personaService.Persona_XIdentPersona(identPersona);
                personaList.TipoEstadoCivil = await _tablasService.ListarTipoEstadoCivil();
                personaList.ListDepartamento = await _tablasService.ListarDepartamento();
                personaList.TipoSexos = await _tablasService.ListarSexo();
                personaList.Paises = await _tablasService.ListarPaises();
                if (mensaje != null)
                {
                    personaList.Mensaje = mensaje;
                }
                return View(personaList);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PersonaActualizar(PersonaList personaList)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (personaList.FechaNacimiento < new DateTime(1900, 1, 1))
                {
                    personaList.FechaNacimiento = DateTime.Now;
                }
                personaList = await _personaService.PersonaActualizar(personaList, loginResult);
                return RedirectToAction("Actualizar", "Persona", new { identPersona = personaList.Ident_Persona, mensaje = personaList.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        public async Task<IActionResult> PersonaAnular(int ident_Persona)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                string mensaje = "";
                mensaje = await _personaService.PersonaAnular(ident_Persona, loginResult);
                return RedirectToAction("Actualizar", "Persona", new { identPersona = ident_Persona, mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        
        [Authorize]
        public async Task<IActionResult> CargarProvincias(string departamento)
        {
            var provinciaList = await _tablasService.ListarProvincia(departamento);
            return Json(provinciaList);
        }
        [Authorize]
        public async Task<IActionResult> CargarDistritos(string departamento, string provincia)
        {
            var distritoLists = await _tablasService.ListarDistrito(departamento, provincia);
            return Json(distritoLists);
        }
    }
}
