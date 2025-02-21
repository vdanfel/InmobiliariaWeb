using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Roles;
using InmobiliariaWeb.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class RolesController:Controller
    {
        private readonly IRolesService _rolesService;
        public RolesController(IRolesService rolesService) 
        {
            _rolesService = rolesService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var rolesIndexViewModel = new RolesIndexViewModel();
                rolesIndexViewModel.RolesList = await _rolesService.ListarRoles(0);
                return View(rolesIndexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(RolesIndexViewModel rolesIndexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                rolesIndexViewModel = await _rolesService.CrearRol(rolesIndexViewModel);
                if (rolesIndexViewModel.Ident_005_TipoUsuario > 0)
                {
                    await _rolesService.CrearAccesos(rolesIndexViewModel, loginResult);
                }
                rolesIndexViewModel.RolesList = await _rolesService.ListarRoles(0);
                return View(rolesIndexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [Authorize]
        public async Task<IActionResult> Gestionar(int Ident_005_TipoUsuario)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ViewGestionar viewGestionar = new ViewGestionar();
                viewGestionar.RolesList = await _rolesService.ListarRoles(Ident_005_TipoUsuario);
                viewGestionar.NombreRol = viewGestionar.RolesList.First().Descripcion;
                viewGestionar.Ident_005_tipoUsuario = viewGestionar.RolesList.First().Ident_005_Rolusuario;
                viewGestionar.PaginasLists = await _rolesService.ListarAccesos(Ident_005_TipoUsuario);
                return View(viewGestionar);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        
    }
}
