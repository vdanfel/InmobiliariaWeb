using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Kardex;
using InmobiliariaWeb.Models.Tablas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    [Authorize]
    public class KardexController:Controller
    {
        private readonly IKardexService _kardexService;
        private readonly IAdendasService _adendasService;
        private readonly IContratosService _contratosService;
        public KardexController(IKardexService kardexService, IAdendasService adendasService, IContratosService contratosService)
        {
            _kardexService = kardexService;
            _adendasService = adendasService;
            _contratosService = contratosService;
        }
        [HttpGet]
        public async Task<IActionResult> Adenda(int nIdent_Contratos, string mensaje = null)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            if (nIdent_Contratos > 0)
            {
                HttpContext.Session.SetInt32("Ident_Contratos", nIdent_Contratos);
            }
            else
            {
                nIdent_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contratos");
            }

            var adendaPendiente = await _adendasService.ObtenerAdendaPendiente(nIdent_Contratos);
            if (adendaPendiente != null)
            {
                // Guardar en sesión el Id de la Adenda
                HttpContext.Session.SetInt32("nIdent_Adendas", adendaPendiente.nIdent_Adendas);

                var model = new AdendasDTO
                {
                    nIdent_Adendas = adendaPendiente.nIdent_Adendas,
                    nIdent_Contratos = nIdent_Contratos,
                    sTextoAdenda = adendaPendiente.sTextoAdenda,
                    nIdent_023_EstadoAdenda = adendaPendiente.nIdent_023_EstadoAdenda
                };

                if (!string.IsNullOrEmpty(mensaje))
                {
                    ViewData["Mensaje"] = mensaje;
                }
                else
                {
                    ViewData["Mensaje"] = "Ya existe una adenda pendiente. Puede continuar editándola.";
                }
                ViewData["ActiveTab"] = "Adenda";
                return View(model);
            }

            else
            {
                // Si aún no hay adenda pendiente, limpiar variable en sesión
                HttpContext.Session.Remove("nIdent_Adendas");
                var nuevaAdenda = new AdendasDTO
                {
                    nIdent_Contratos = nIdent_Contratos,
                    sTextoAdenda = "",
                };

                if (!string.IsNullOrEmpty(mensaje))
                {
                    ViewData["Mensaje"] = mensaje;
                }

                ViewData["ActiveTab"] = "Adenda";

                return View(nuevaAdenda);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Adenda(AdendasDTO adendasDTO)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }

            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            adendasDTO.nUsuarioCreacion = loginResult.IdentUsuario;
            adendasDTO.nUsuarioModificacion = loginResult.IdentUsuario;

            if (adendasDTO.nIdent_Adendas < 1)
            {
                // Insertar adenda
                adendasDTO.nIdent_Adendas = await _adendasService.AdendasCreate(adendasDTO);
            }
            else
            {
                //Actualizar adenda
                var res = await _adendasService.AdendasUpdate(adendasDTO);
            }
            // Redirigir al GET con mensaje
            return RedirectToAction("Adenda", new { nIdent_Contratos = adendasDTO.nIdent_Contratos, mensaje = "Adenda creada correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo(int nIdent_Contratos)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            if (nIdent_Contratos > 0)
            {
                HttpContext.Session.SetInt32("Ident_Contratos", nIdent_Contratos);
            }
            else
            {
                nIdent_Contratos = (int)HttpContext.Session.GetInt32("Ident_Contratos");
            }
            KardexNuevoDTO kardexNuevoDTO = await _kardexService.DatosAdenda(nIdent_Contratos);
            kardexNuevoDTO.nImporteNuevo = kardexNuevoDTO.nTotalDeuda;
            kardexNuevoDTO.dFechaInicio = DateTime.Now;
            ViewData["ActiveTab"] = "Nuevo";
            return View(kardexNuevoDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo(KardexNuevoDTO kardexNuevoDTO)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);

            kardexNuevoDTO.nIdent_Adendas = (int)HttpContext.Session.GetInt32("nIdent_Adendas");
            kardexNuevoDTO.nUsuarioCreacion = loginResult.IdentUsuario;

            int nIdent_Kardex = await _kardexService.KardexNuevoInsert(kardexNuevoDTO);

            if (nIdent_Kardex > 0)
            {
                HttpContext.Session.SetInt32("Ident_Kardex", nIdent_Kardex);
            }
            else
            {
                HttpContext.Session.Remove("Ident_Kardex");
            }
            return RedirectToAction("Kardex", "Contratos");
        }
        public async Task<IActionResult> DescargarWord(int nIdent_Adendas)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }

            var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
            if (nIdent_Adendas > 0)
            {
                HttpContext.Session.SetInt32("nIdent_Adendas", nIdent_Adendas);
            }
            else
            {
                nIdent_Adendas = (int)HttpContext.Session.GetInt32("nIdent_Adendas");
            }

            var adendasDTO= await _adendasService.AdendasSelect(nIdent_Adendas);

            if (adendasDTO == null)
            {
                return RedirectToAction("Adenda", "Kardex",
                    new { mensaje = "no se encontro el formato de la adenda" });
            }
            string fileName = "Adenda" + nIdent_Adendas + ".doc";

            string wordContent = $@"
            <html xmlns:o='urn:schemas-microsoft-com:office:office' 
                  xmlns:w='urn:schemas-microsoft-com:office:word'
                  xmlns='http://www.w3.org/TR/REC-html40'>
            <head>
                <meta charset='utf-8'/>
                <title>Exportar a Word</title>
                <xml>
                    <w:WordDocument>
                        <w:View>Print</w:View>
                        <w:Zoom>100</w:Zoom>
                        <w:DoNotOptimizeForBrowser/>
                    </w:WordDocument>
                </xml>
                <style>
                    body{{
                        margin-left:-10px
                        margin-right:-10px
                    }}
                    p {{
                        margin-bottom:5px;
                        font-size: 16px;
                        text-align: justify;
                        font-family: 'Arial Narrow';
                        line-height: 13px;
                    }}

                    .Titulo {{
                        font-weight: bold;
                        font-size: 21px;
                        text-align: center;
                        font-family: 'Arial Narrow';
                        margin-bottom:15px;
                    }}

                    .Parrafo {{
                        font-size: 16px;
                        text-align: justify;
                        font-family: 'Arial Narrow';
                        margin-top:10px;
                        margin-left:-10px
                        margin-right:-10px
                    }}

                    .espacio {{
                        margin: 20px 0;
                    }}

                    .datos {{
                        text-align: center;
                        font-size: 12px;
                        font-weight: bold;
                        font-family: 'Arial Narrow';
                    }}

                    .tabla-involucrados {{
                        width: 100%;
                        border-collapse: collapse;
                        border-spacing: 0;
                    }}

                    .tabla-involucrados td {{
                        border: none;
                        padding: 0;
                        font-family: 'Arial Narrow';
                    }}

                    b, strong {{
                        font-family: 'Arial Narrow';
                    }}

                    sup {{
                        font-family: 'Arial Narrow';
                        font-size: 0.8em;
                    }}
                </style>
            </head>
            <body>
                {adendasDTO.sTextoAdenda}
            </body>
            </html>";

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(wordContent);

            return File(byteArray, "application/msword", fileName);
        }
    }
}
