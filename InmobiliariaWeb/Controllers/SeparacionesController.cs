using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Separaciones;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using ClienteViewModel = InmobiliariaWeb.Models.Separaciones.ClienteViewModel;

namespace InmobiliariaWeb.Controllers
{
    public class SeparacionesController:Controller
    {
        private readonly ISeparacionesService _separacionesService;
        private readonly ITablasService _tablasService;
        private readonly ICajaService _cajaService;
        public SeparacionesController(ISeparacionesService separacionesService, ITablasService tablasService, ICajaService cajaService)
        {
            _separacionesService = separacionesService;
            _tablasService = tablasService;
            _cajaService = cajaService; 
        }
        [Authorize]
        public async Task<IActionResult> Imprimir(int ident_Separacion, int ident_010_TipoLote)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                byte[] pdfBytes;
                pdfBytes = await PDFImprimir(ident_Separacion, ident_010_TipoLote);
                return File(pdfBytes, "application/pdf", "archivo.pdf");
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [Authorize]
        public async Task<byte[]> PDFImprimir(int ident_Separacion, int ident_010_tipoLote)
        {
            SeparacionesImpresionViewModel separacionesImpresionViewModel = await _separacionesService.ImprimirSeparaciones(ident_Separacion);
            // Crear un documento PDF
            using (PdfDocument document = new PdfDocument())
            {
                // Agregar una página al documento
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 10, XFontStyle.Regular);
                XFont fontNumeroSerie = new XFont("Arial", 18, XFontStyle.Bold);
                XFont encabezados = new XFont("Arial",11, XFontStyle.Italic);
                XFont datos = new XFont("Arial", 12, XFontStyle.Bold);
                XFont superponer = new XFont("Arial", 7, XFontStyle.Italic);

                // Definir márgenes y tamaño de página
                XRect borde = new XRect(40, 40, page.Width - 80, page.Height - 80);
                // Dibujar un borde alrededor de toda la página
                gfx.DrawRectangle(XPens.Black, borde.Left, borde.Top, borde.Width, borde.Height);

                XRect numeroSerie = new XRect(145, 60, 305, 40);
                gfx.DrawString("Separacion: " + separacionesImpresionViewModel.NumeroSerie.ToString(), fontNumeroSerie, XBrushes.Black, numeroSerie, XStringFormats.Center);

                XRect textoFechaCreacion = new XRect(450, 55, 90, 15);
                gfx.DrawString("Fecha Separación", encabezados, XBrushes.Black, textoFechaCreacion, XStringFormats.CenterRight);
                XRect fechaCreacion = new XRect(450, 70, 90, 20);
                gfx.DrawString(separacionesImpresionViewModel.FechaSeparacion.ToString("dd/MM/yyyy"), datos, XBrushes.Black, fechaCreacion, XStringFormats.CenterRight);

                XRect textoNombrePrograma = new XRect(55, 110, 420, 15);
                gfx.DrawString("Programa:", encabezados, XBrushes.Black, textoNombrePrograma, XStringFormats.CenterLeft);
                XRect nombrePrograma = new XRect(55, 125, 420, 20);
                gfx.DrawString(separacionesImpresionViewModel.NombrePrograma.ToString(), datos, XBrushes.Black, nombrePrograma, XStringFormats.CenterLeft);
                XRect textoManzana = new XRect(475, 110, 30, 15);
                gfx.DrawString("Mz:", encabezados, XBrushes.Black, textoManzana, XStringFormats.CenterLeft);
                XRect manzana = new XRect(475, 125, 30, 20);
                gfx.DrawString(separacionesImpresionViewModel.Manzana.ToString(), datos, XBrushes.Black, manzana, XStringFormats.CenterLeft);
                XRect textoLote = new XRect(505, 110, 30, 15);
                gfx.DrawString("Lt:", encabezados, XBrushes.Black, textoLote, XStringFormats.CenterLeft);
                XRect lote = new XRect(505, 125, 30, 20);
                gfx.DrawString(separacionesImpresionViewModel.Lote.ToString(), datos, XBrushes.Black, lote, XStringFormats.CenterLeft);

                XRect textoArea = new XRect(55, 150, 35, 20);
                gfx.DrawString("Area:", encabezados, XBrushes.Black, textoArea, XStringFormats.CenterLeft);
                XRect area = new XRect(90, 150, 70, 20);
                gfx.DrawString(separacionesImpresionViewModel.Area.ToString("#,##0.00"), datos, XBrushes.Black, area, XStringFormats.CenterLeft);
                XRect textoPrecioM2 = new XRect(160, 150, 25, 20);
                gfx.DrawString("$m :", encabezados, XBrushes.Black, textoPrecioM2, XStringFormats.CenterLeft);
                XRect textoPrecioM2superponer = new XRect(176, 148, 35, 20);
                gfx.DrawString("2", superponer, XBrushes.Black, textoPrecioM2superponer, XStringFormats.CenterLeft);
                XRect precioM2 = new XRect(190, 150, 70, 20);
                gfx.DrawString(separacionesImpresionViewModel.PrecioM2.ToString("#,##0.00"), datos, XBrushes.Black, precioM2, XStringFormats.CenterLeft);
                XRect textoPrecioTotal = new XRect(250, 150, 40, 20);
                gfx.DrawString("$ Total:", encabezados, XBrushes.Black, textoPrecioTotal, XStringFormats.CenterLeft);
                XRect precioTotal = new XRect(295, 150, 70, 20);
                gfx.DrawString(separacionesImpresionViewModel.PrecioTotal.ToString("#,##0.00"), datos, XBrushes.Black, precioTotal, XStringFormats.CenterLeft);
                XRect textoUbicacion = new XRect(375, 150, 40, 20);
                gfx.DrawString("Ubicacion:", encabezados, XBrushes.Black, textoUbicacion, XStringFormats.CenterLeft);
                XRect ubicacion = new XRect(435, 150, 70, 20);
                gfx.DrawString(separacionesImpresionViewModel.Ubicacion.ToString(), datos, XBrushes.Black, ubicacion, XStringFormats.CenterLeft);

                XRect textoMedidasLt = new XRect(55,190,200,20);
                gfx.DrawString("Medidas del Lote:", encabezados,XBrushes.Black, textoMedidasLt, XStringFormats.CenterLeft);
                if (ident_010_tipoLote == 84)
                {
                    XRect textoIzquierda = new XRect(55, 205, 100, 15);
                    gfx.DrawString("Izquierda:", encabezados, XBrushes.Black, textoIzquierda, XStringFormats.CenterLeft);
                    XRect textoDerecha = new XRect(155, 205, 100, 15);
                    gfx.DrawString("Derecha:", encabezados, XBrushes.Black, textoDerecha, XStringFormats.CenterLeft);
                    XRect textoFrente = new XRect(255, 205, 100, 15);
                    gfx.DrawString("Frente:", encabezados, XBrushes.Black, textoFrente, XStringFormats.CenterLeft);
                    XRect textoFondo = new XRect(355, 205, 100, 15);
                    gfx.DrawString("Fondo:", encabezados, XBrushes.Black, textoFondo, XStringFormats.CenterLeft);

                    XRect izquierda = new XRect(55, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.Izquierda.ToString("#,##0.00"), datos, XBrushes.Black, izquierda, XStringFormats.CenterLeft);
                    XRect derecha = new XRect(155, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.Derecha.ToString("#,##0.00"), datos, XBrushes.Black, derecha, XStringFormats.CenterLeft);
                    XRect frente = new XRect(255, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.Frente.ToString("#,##0.00"), datos, XBrushes.Black, frente, XStringFormats.CenterLeft);
                    XRect fondo = new XRect(355, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.Fondo.ToString("#,##0.00"), datos, XBrushes.Black, fondo, XStringFormats.CenterLeft);
                }
                else 
                {
                    XRect textoL1 = new XRect(55, 205, 45, 15);
                    gfx.DrawString("L1:", encabezados, XBrushes.Black, textoL1, XStringFormats.CenterLeft);
                    XRect textoL2 = new XRect(100, 205, 45, 15);
                    gfx.DrawString("L2:", encabezados, XBrushes.Black, textoL2, XStringFormats.CenterLeft);
                    XRect textoL3 = new XRect(145, 205, 45, 15);
                    gfx.DrawString("L3:", encabezados, XBrushes.Black, textoL3, XStringFormats.CenterLeft);
                    XRect textoL4 = new XRect(190, 205, 45, 15);
                    gfx.DrawString("L4:", encabezados, XBrushes.Black, textoL4, XStringFormats.CenterLeft);
                    XRect textoL5 = new XRect(235, 205, 45, 15);
                    gfx.DrawString("L5:", encabezados, XBrushes.Black, textoL5, XStringFormats.CenterLeft);
                    XRect textoL6 = new XRect(280, 205, 45, 15);
                    gfx.DrawString("L6:", encabezados, XBrushes.Black, textoL6, XStringFormats.CenterLeft);
                    XRect textoL7 = new XRect(325, 205, 45, 15);
                    gfx.DrawString("L7:", encabezados, XBrushes.Black, textoL7, XStringFormats.CenterLeft);
                    XRect textoL8 = new XRect(370, 205, 45, 15);
                    gfx.DrawString("L8:", encabezados, XBrushes.Black, textoL8, XStringFormats.CenterLeft);
                    XRect textoL9 = new XRect(415, 205, 45, 15);
                    gfx.DrawString("L9:", encabezados, XBrushes.Black, textoL9, XStringFormats.CenterLeft);
                    XRect textoL10 = new XRect(460, 205, 45, 15);
                    gfx.DrawString("L10:", encabezados, XBrushes.Black, textoL10, XStringFormats.CenterLeft);

                    XRect l1 = new XRect(55, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L1.ToString("#,##0.00"), datos, XBrushes.Black, l1, XStringFormats.CenterLeft);
                    XRect l2 = new XRect(100, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L2.ToString("#,##0.00"), datos, XBrushes.Black, l2, XStringFormats.CenterLeft);
                    XRect l3 = new XRect(145, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L3.ToString("#,##0.00"), datos, XBrushes.Black, l3, XStringFormats.CenterLeft);
                    XRect l4 = new XRect(190, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L4.ToString("#,##0.00"), datos, XBrushes.Black, l4, XStringFormats.CenterLeft);
                    XRect l5 = new XRect(235, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L5.ToString("#,##0.00"), datos, XBrushes.Black, l5, XStringFormats.CenterLeft);
                    XRect l6 = new XRect(280, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L6.ToString("#,##0.00"), datos, XBrushes.Black, l6, XStringFormats.CenterLeft);
                    XRect l7 = new XRect(325, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L7.ToString("#,##0.00"), datos, XBrushes.Black, l7, XStringFormats.CenterLeft);
                    XRect l8 = new XRect(370, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L8.ToString("#,##0.00"), datos, XBrushes.Black, l8, XStringFormats.CenterLeft);
                    XRect l9 = new XRect(415, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L9.ToString("#,##0.00"), datos, XBrushes.Black, l9, XStringFormats.CenterLeft);
                    XRect l10 = new XRect(460, 220, 45, 20);
                    gfx.DrawString(separacionesImpresionViewModel.L10.ToString("#,##0.00"), datos, XBrushes.Black, l10, XStringFormats.CenterLeft);
                }

                XRect textoTratadoEn = new XRect(55,250,70, 20);
                gfx.DrawString("Tratado En:", encabezados, XBrushes.Black, textoTratadoEn, XStringFormats.CenterLeft);
                XRect tratadoEn = new XRect(120, 250, 100, 20);
                gfx.DrawString(separacionesImpresionViewModel.TratadoEn.ToString("#,##0.00"), datos, XBrushes.Black, tratadoEn, XStringFormats.CenterLeft);
                XRect textoCuotaInicial = new XRect(220, 250,70, 20);
                gfx.DrawString("Cuota Inicial:", encabezados, XBrushes.Black, textoCuotaInicial, XStringFormats.CenterLeft);
                XRect cuotaInicial = new XRect(300, 250, 100, 20);
                gfx.DrawString(separacionesImpresionViewModel.CuotaInicial.ToString("#,##0.00"), datos, XBrushes.Black, cuotaInicial, XStringFormats.CenterLeft);
                XRect textoSaldo = new XRect(400, 250, 50, 20);
                gfx.DrawString("Saldo:", encabezados, XBrushes.Black, textoSaldo, XStringFormats.CenterLeft);
                XRect saldo = new XRect(450, 250, 100, 20);
                gfx.DrawString(separacionesImpresionViewModel.SaldoAPagar.ToString("#,##0.00"), datos, XBrushes.Black, saldo, XStringFormats.CenterLeft);

                XRect textoNombreCliente = new XRect(55, 300, 300, 15);
                gfx.DrawRectangle(XPens.Black, textoNombreCliente);
                gfx.DrawString(" Apellidos y Nombres", encabezados, XBrushes.Black, textoNombreCliente, XStringFormats.CenterLeft);
                XRect textoTipoDocumento = new XRect(355, 300, 90, 15);
                gfx.DrawRectangle(XPens.Black, textoTipoDocumento);
                gfx.DrawString(" Tipo Documento", encabezados, XBrushes.Black, textoTipoDocumento, XStringFormats.CenterLeft);
                XRect textoNroDocumento = new XRect(445, 300, 90, 15);
                gfx.DrawRectangle(XPens.Black, textoNroDocumento);
                gfx.DrawString(" Nro Documento", encabezados, XBrushes.Black, textoNroDocumento, XStringFormats.CenterLeft);

                int top = 315; // Posición inicial del primer nombre
                int incremento = 20; // Incremento de posición para cada nombre
                foreach (var cliente in separacionesImpresionViewModel.Clientes)
                {
                    XRect nombreCliente = new XRect(55, top, 300, 20);
                    gfx.DrawRectangle(XPens.Black, nombreCliente);
                    gfx.DrawString(" "+cliente.NombreCliente, datos, XBrushes.Black, nombreCliente, XStringFormats.CenterLeft);
                    XRect tipoDocumento = new XRect(355, top, 90, 20);
                    gfx.DrawRectangle(XPens.Black, tipoDocumento);
                    gfx.DrawString(cliente.TipoDocumento, datos, XBrushes.Black, tipoDocumento, XStringFormats.Center);
                    XRect documento = new XRect(445, top, 90, 20);
                    gfx.DrawRectangle(XPens.Black, documento);
                    gfx.DrawString(cliente.Documento, datos, XBrushes.Black, documento, XStringFormats.Center);
                    top += incremento; // Aumentar la posición para el siguiente nombre
                }

                XRect textoObservacion = new XRect(55, 450, 300, 15);
                gfx.DrawString("Observación", encabezados, XBrushes.Black, textoObservacion, XStringFormats.CenterLeft);

                XRect observacion = new XRect(55, 465, 480, 60);
                XTextFormatter tf = new XTextFormatter(gfx);
                tf.DrawString(separacionesImpresionViewModel.Observacion.ToString(), datos, XBrushes.Black, observacion, XStringFormats.TopLeft);

                XRect textImporteSeparacion = new XRect(55, 525, 100, 20);
                gfx.DrawString("Importe Separación:", encabezados, XBrushes.Black, textImporteSeparacion, XStringFormats.CenterLeft);
                XRect importeSeparacion = new XRect(165, 525, 100, 20);
                gfx.DrawString(separacionesImpresionViewModel.ImporteSeparacion.ToString("#,##0.00"), datos, XBrushes.Black, importeSeparacion, XStringFormats.CenterLeft);
                XRect textoFechaVencimiento = new XRect(300, 525, 100, 20);
                gfx.DrawString("Fecha Vencimiento:", encabezados, XBrushes.Black, textoFechaVencimiento, XStringFormats.CenterLeft);
                XRect fechaVencimiento = new XRect(400, 525, 100, 20);
                gfx.DrawString(separacionesImpresionViewModel.FechaVencimiento.ToString("dd/MM/yyyy"), datos, XBrushes.Black, fechaVencimiento, XStringFormats.CenterLeft);

                // Guardar el documento en la ruta temporal
                string tempFilePath = Path.GetTempFileName();
                document.Save(tempFilePath);

                // Leer el archivo PDF en bytes
                byte[] pdfBytes = System.IO.File.ReadAllBytes(tempFilePath);

                // Devolver los bytes del archivo PDF
                return pdfBytes;
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index() 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                //await _separacionesService.AnularSeparacionesVencidas();
                IndexViewModel indexViewModel = new IndexViewModel();
                indexViewModel.PaginaActual = 1;
                indexViewModel.GrupoActual = 1;
                indexViewModel.Correlativo = 0;
                indexViewModel.Ident_Programa = 0;
                indexViewModel.Ident_Manzana = 0;
                indexViewModel.SeparacionesLists = await _separacionesService.BandejaSeparaciones(indexViewModel);
                indexViewModel.ProgramasCbxLists = await _separacionesService.ProgramaCbxListar();
                indexViewModel.NumeroGrupos = (int)Math.Ceiling((double)indexViewModel.NumeroPaginas / 10);
                return View(indexViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(IndexViewModel indexViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                indexViewModel.SeparacionesLists = await _separacionesService.BandejaSeparaciones(indexViewModel);
                indexViewModel.ProgramasCbxLists = await _separacionesService.ProgramaCbxListar();
                indexViewModel.NumeroGrupos = (int)Math.Ceiling((double)indexViewModel.NumeroPaginas / 10);
                indexViewModel.GrupoActual = (int)Math.Ceiling((double)indexViewModel.PaginaActual / 10);
                return View(indexViewModel);
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
                CrearViewModel crearViewModel = new CrearViewModel();
                crearViewModel.ProgramasCbxLists = await _separacionesService.ProgramaCbxListar();
                return View(crearViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear(CrearViewModel crearViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var ident_Separacion = await _separacionesService.SeparacionesInsert(crearViewModel, loginResult);
                HttpContext.Session.SetInt32("IdentSeparacion", ident_Separacion);
                crearViewModel.Ident_Separaciones = ident_Separacion;
                return RedirectToAction("Actualizar", "Separaciones", new { Ident_Separacion = ident_Separacion });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetManzanas(int programaId)
        {
            var manzanas = await _separacionesService.ManzanaCbxListar(programaId);
            return Json(manzanas);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLotes(int manzanaId)
        {
            var lotes = await _separacionesService.LoteCbxListar(manzanaId);
            return Json(lotes);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLoteDetalle(int ident_Lote)
        {
            var lotes = await _separacionesService.LoteDetalle(ident_Lote);
            return Json(lotes);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Actualizar(int Ident_Separacion,string Mensaje) 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var actualizarViewModel = new ActualizarViewModel();
                if (Ident_Separacion == 0)
                {
                    Ident_Separacion = (int)HttpContext.Session.GetInt32("IdentSeparacion");
                }
                else
                {
                    HttpContext.Session.SetInt32("IdentSeparacion", Ident_Separacion);
                }
                actualizarViewModel = await _separacionesService.SeparacionXIdentSeparacion(Ident_Separacion);
                if (Mensaje != null || Mensaje != "")
                {
                    actualizarViewModel.Mensaje = Mensaje;
                }
                HttpContext.Session.SetInt32("Ident_010_TipoLote", actualizarViewModel.ident_010_TipoLote);
                HttpContext.Session.SetString("Numero_Separacion", actualizarViewModel.Numero_Separacion);
                return View(actualizarViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(ActualizarViewModel actualizarViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (actualizarViewModel.MotivoActualizacion == null || actualizarViewModel.MotivoActualizacion == "")
                {
                    actualizarViewModel.Mensaje = "Motivo de Actualización no puede estar en blanco";
                    return RedirectToAction("Actualizar", "Separaciones", new { Ident_Separacion = actualizarViewModel.Ident_Separacion, Mensaje = actualizarViewModel.Mensaje });
                }

                actualizarViewModel.Mensaje = await _separacionesService.ActualizarSeparacion(actualizarViewModel, loginResult);
                //HttpContext.Session.SetString("Numero_Separacion", actualizarViewModel.Numero_Separacion);
                //HttpContext.Session.SetInt32("Ident_010_TipoLote", actualizarViewModel.ident_010_TipoLote);
                return RedirectToAction("Actualizar", "Separaciones", new { Ident_Separacion = actualizarViewModel.Ident_Separacion, Mensaje = actualizarViewModel.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cliente(int Ident_Separacion, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                ClienteViewModel clienteViewModel = new ClienteViewModel();
                if (Ident_Separacion == 0)
                {
                    Ident_Separacion = (int)HttpContext.Session.GetInt32("IdentSeparacion");
                }
                else
                {
                    HttpContext.Session.SetInt32("IdentSeparacion", Ident_Separacion);
                }
                if (Mensaje != null)
                {
                    clienteViewModel.Mensaje = Mensaje;
                }

                clienteViewModel.Ident_Separaciones = Ident_Separacion;
                clienteViewModel.Clientes = await _separacionesService.ClientexSeparacion(clienteViewModel);
                clienteViewModel.Numero_Separacion = HttpContext.Session.GetString("Numero_Separacion");
                ViewBag.Ident_010_TipoLote = HttpContext.Session.GetInt32("Ident_010_TipoLote");
                return View(clienteViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cliente(ClienteViewModel clienteViewModel) 
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (clienteViewModel.Ident_Persona > 0)
                {
                    clienteViewModel.Mensaje = await _separacionesService.ClienteInsertar(clienteViewModel, loginResult);
                }
                else
                {
                    clienteViewModel.Mensaje = "debe seleccionar un cliente";
                }
                ViewBag.Ident_010_TipoLote = HttpContext.Session.GetInt32("Ident_010_TipoLote");
                return RedirectToAction("Cliente", "Separaciones", new { Ident_Separacion = clienteViewModel.Ident_Separaciones, Mensaje = clienteViewModel.Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            

        }
        [Authorize]
        public async Task<IActionResult> EliminarCliente(int Ident_Separacion, int Ident_SeparacionesCliente)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                var Mensaje = await _separacionesService.ClienteEliminar(Ident_SeparacionesCliente, loginResult);
                return RedirectToAction("Cliente", "Separaciones", new { Ident_Separacion = Ident_Separacion, Mensaje = Mensaje });
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Anular(int Ident_Separacion, string Mensaje)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                if (Ident_Separacion == 0)
                {
                    Ident_Separacion = (int)HttpContext.Session.GetInt32("IdentSeparacion");
                }
                else
                {
                    HttpContext.Session.SetInt32("IdentSeparacion", Ident_Separacion);
                }
                ClienteViewModel clienteViewModel = new ClienteViewModel();
                clienteViewModel.Ident_Separaciones = Ident_Separacion;
                var actualizarViewModel = await _separacionesService.SeparacionXIdentSeparacion(Ident_Separacion);
                ViewBag.Clientes = await _separacionesService.ClientexSeparacion(clienteViewModel);
                return View(actualizarViewModel);
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Anular(ActualizarViewModel actualizarViewModel)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                var loginResult = DatosLogin.DatosUsuarioLogin(HttpContext);
                await _separacionesService.SeparacionesAnular(actualizarViewModel, loginResult);
                return RedirectToAction("Index", "Separaciones");
            }
            else
            {
                return RedirectToAction("Alerta", "Login", new { Mensaje = "Su sesión expiró, vuelva a iniciar sesión" });
            }   
        }
        

    }
}
