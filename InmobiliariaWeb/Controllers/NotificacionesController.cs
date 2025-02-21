using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Notificaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;

namespace InmobiliariaWeb.Controllers
{
    public class NotificacionesController:Controller
    {
        private readonly INotificacionesService _notificacionesService;
        private readonly IConfiguration _configuration;
        public NotificacionesController(INotificacionesService notificacionesService, IConfiguration configuration) 
        {
            _notificacionesService = notificacionesService;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var estadosCliente = _configuration.GetSection("DashboardSettings:EstadosCliente").Get<List<string>>();
                IndexViewModel indexViewModel = new IndexViewModel();
                indexViewModel.ProgramasCbxLists = await _notificacionesService.ProgramaCbxListar();
                indexViewModel.EstadosClienteCbxList = estadosCliente;
                indexViewModel.NombrePersona = "";
                indexViewModel.Ident_Programa = 0;
                indexViewModel.Manzana = "";
                indexViewModel.Estado = "TODOS";
                indexViewModel.NotificacionesLists = await _notificacionesService.NotificacionesListar(indexViewModel);
                return View(indexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> index(IndexViewModel indexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var estadosCliente = _configuration.GetSection("DashboardSettings:EstadosCliente").Get<List<string>>();
                indexViewModel.ProgramasCbxLists = await _notificacionesService.ProgramaCbxListar();
                indexViewModel.EstadosClienteCbxList = estadosCliente;
                indexViewModel.NotificacionesLists = await _notificacionesService.NotificacionesListar(indexViewModel);
                return View(indexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfirmarNotificacion(int Ident_Cuotas, int Ident_Persona)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                try
                {
                    var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                    await _notificacionesService.ConfirmarNotificacion(Ident_Cuotas,Ident_Persona, loginResult.IdentUsuario);
                    return NoContent(); // Retorna un 204
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Ocurrió un error interno.");
                }
            }
            else
            {
                return Unauthorized(); // Retorna un 401 si la sesión expiró
            }
        }
        public async Task<IActionResult> ExportarNotificaciones(string NombrePersona, int Ident_Programa, string Manzana, string Estado)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var exceldatos = await _notificacionesService.ExportarExcel(NombrePersona,Ident_Programa, Manzana, Estado);
                // Construir tabla HTML
                string htmlTable = @"
                <html>
                <head>
                    <meta charset='UTF-8'> <!-- Especificar UTF-8 -->
                    <style>
                        table {
                            border-collapse: collapse;
                            width: 100%;
                        }
                        th, td {
                            border: 1px solid black;
                            padding: 8px;
                            text-align: left;
                        }
                        th {
                            background-color: #f2f2f2;
                        }
                    </style>
                </head>
                <body>
                    <table>
                        <thead>
                            <tr>
                                <th>N°</th>
                                <th>Apellidos y Nombres</th>
                                <th>Programa</th>
                                <th>Mz</th>
                                <th>Lt</th>
                                <th>Fecha de Pago</th>
                                <th>Días de Mora</th>
                                <th>Estado Cliente</th>
                                <th>Datos</th>
                                <th>Cantidad de Notificaciones</th>
                            </tr>
                        </thead>
                <tbody>";

                        // Llenar las filas con los datos
                        foreach (var item in exceldatos)
                        {
                            htmlTable += $@"
                            <tr>
                                <td>{item.Indice}</td>
                                <td>{item.NombreCompleto}</td>
                                <td>{item.NombrePrograma}</td>
                                <td>{item.Manzana}</td>
                                <td>{item.Lote}</td>
                                <td>{item.FechaPago:yyyy-MM-dd}</td>
                                <td>{item.DiasMoras}</td>
                                <td>{item.EstadoCliente}</td>
                                <td>{item.Datos}</td>
                                <td>{item.CantidadNotificaciones}</td>
                            </tr>";
                        }

                        htmlTable += @"
                        </tbody>
                    </table>
                </body>
                </html>";

                // Convertir el HTML a un arreglo de bytes
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(htmlTable);

                // Retornar el archivo Excel
                string fileName = "Notificaciones.xls";
                return File(byteArray, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }


    }
}
