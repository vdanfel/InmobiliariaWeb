using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class DashboardController:Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IConfiguration _configuration;
        public DashboardController(IDashboardService dashboardService, IConfiguration configuration) 
        {
            _dashboardService = dashboardService;
            _configuration = configuration;
        }
        [Authorize]
        public async Task<IActionResult> Index(string Mensaje) 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                dashboardViewModel.Mensaje = Mensaje;
                dashboardViewModel.ProgramasCbxLists = await _dashboardService.ProgramaCbxListar();
                // Obtenemos la lista de años y estados de cliente desde appsettings.json
                var anios = _configuration.GetSection("DashboardSettings:Anios").Get<List<int>>();
                var estadosCliente = _configuration.GetSection("DashboardSettings:EstadosCliente").Get<List<string>>();

                // Asignamos los valores al ViewModel
                dashboardViewModel.AniosCbxList = anios;
                dashboardViewModel.EstadosClienteCbxList = estadosCliente;

                dashboardViewModel.Year = DateTime.Now.Year;
                dashboardViewModel.Month = DateTime.Now.Month;

                return View(dashboardViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [Authorize]
        public async Task<IActionResult> TotalesProgramas(int Ident_Programa, int Anio, int Mes)
        {
            var totalesProgramas = await _dashboardService.TTotalesProgramas(Ident_Programa,Anio,Mes);
            return Json(totalesProgramas);
        
        }
        [Authorize]
        public async Task<IActionResult> VSPeriodos(int Ident_Programa, int Anio, int Mes)
        { 
            var vsPeriodos = await _dashboardService.TVSPeriodos(Ident_Programa,Anio,Mes);
            return Json(vsPeriodos);
        }
    }
}
