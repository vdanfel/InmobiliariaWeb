using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Usuario;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Usuario;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class UsuarioController:Controller
    {
        private readonly ITablasService _tablasService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPersonaService _personaService;

        public UsuarioController(ITablasService tablasService, IUsuarioService usuarioService, IPersonaService personaService)
        {
            _tablasService = tablasService;
            _usuarioService = usuarioService;
            _personaService = personaService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index() 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
                usuarioViewModel.UsuarioList = await _usuarioService.ListarUsuario("");
                return View(usuarioViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(UsuarioViewModel usuarioViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                usuarioViewModel.UsuarioList = await _usuarioService.ListarUsuario(usuarioViewModel.Buscar);
                return View(usuarioViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Actualizar(int Ident_Usuario,string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                UsuarioList usuarioList = new UsuarioList();
                usuarioList = await _usuarioService.ListarUsuario_xIdentUsuario(Ident_Usuario);
                usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                if (Mensaje != null || Mensaje != "")
                {
                    usuarioList.Mensaje = Mensaje;
                }
                return View(usuarioList);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ActualizarRol(UsuarioList usuarioList)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (usuarioList.Clave1 != usuarioList.Clave2)
                {
                    usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                    usuarioList.Mensaje = "Las claves no son iguales";
                    return RedirectToAction("Actualizar", "Usuario", new { Ident_Usuario = usuarioList.Ident_Usuario, Mensaje = usuarioList.Mensaje });
                }
                usuarioList.Mensaje = await _usuarioService.ActualizarUsuario(usuarioList, loginResult, 1);
                usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                return RedirectToAction("Actualizar", "Usuario", new { Ident_Usuario = usuarioList.Ident_Usuario, Mensaje = usuarioList.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ActualizarClave(UsuarioList usuarioList)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (usuarioList.Clave1 != usuarioList.Clave2)
                {
                    usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                    usuarioList.Mensaje = "Las claves no son iguales";
                    return RedirectToAction("Actualizar", "Usuario", new { Ident_Usuario = usuarioList.Ident_Usuario, Mensaje = usuarioList.Mensaje });
                }
                usuarioList.Mensaje = await _usuarioService.ActualizarUsuario(usuarioList, loginResult, 2);
                usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                return RedirectToAction("Actualizar", "Usuario", new { Ident_Usuario = usuarioList.Ident_Usuario, Mensaje = usuarioList.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RestaurarClave(UsuarioList usuarioList)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                usuarioList.Mensaje = await _usuarioService.ActualizarUsuario(usuarioList, loginResult, 3);
                usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                return RedirectToAction("Actualizar", "Usuario", new { Ident_Usuario = usuarioList.Ident_Usuario, Mensaje = usuarioList.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
           
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AnularUsuario(UsuarioList usuarioList)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                usuarioList.Mensaje = await _usuarioService.AnularUsuario(usuarioList, loginResult);
                return RedirectToAction("Index", "Usuario");
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
                UsuarioList usuarioList = new UsuarioList();
                usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                return View(usuarioList);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear(UsuarioList usuarioList)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var mensaje = "";
                if (usuarioList.IdentPersona == 0)
                {
                    mensaje = "Debe seleccionar a una persona para asociar al usuario";
                    usuarioList.Mensaje = mensaje;
                    usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                    return View(usuarioList);
                }
                if (usuarioList.Clave1 == usuarioList.Clave2)
                {
                    usuarioList.Clave = usuarioList.Clave1;
                }
                else
                {
                    mensaje = "las claves no son iguales";
                    usuarioList.Mensaje = mensaje;
                    usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                    return View(usuarioList);
                }
                usuarioList.Ident_Usuario = await _usuarioService.RegistrarUsuario(usuarioList, loginResult);
                if (usuarioList.Ident_Usuario > 0)
                {
                    mensaje = "Usuario Registrado con éxito";
                    return RedirectToAction("Actualizar", "Usuario", new { Ident_Usuario = usuarioList.Ident_Usuario, Mensaje = mensaje });
                }
                else
                {
                    mensaje = "No se pudo registrar al Usuario";
                    usuarioList.Mensaje = mensaje;
                    usuarioList.TipoRols = await _tablasService.ListarTipoUsuarios();
                    return View(usuarioList);
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
    }
}
