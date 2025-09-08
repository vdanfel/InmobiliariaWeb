using Xceed.Document.NET;
using Xceed.Words.NET;

namespace InmobiliariaWeb.Utilities
{
    public class ReciboGenerador
    {
        private readonly string _templatePath;
        public ReciboGenerador(string webRootPath, int nIdent_021_TipoIngresos)
        {
            if (nIdent_021_TipoIngresos == 135 || nIdent_021_TipoIngresos == 136)
            {
                _templatePath = Path.Combine(webRootPath, "template", "FormatoReciboCuota.docx");
            }
            else
            {
                _templatePath = Path.Combine(webRootPath, "template", "FormatoRecibo.docx");
            }
        }
        public async Task<byte[]> GenerarReciboAsync(ReciboBE datos)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = DocX.Load(_templatePath))
                {
                    var replacements = new Dictionary<string, string>()
                    {
                        ["{NumeroRecibo}"] = datos.NumeroRecibo ?? "",
                        ["{FechaPago}"] = datos.FechaPago.ToString("dd/MM/yyyy"),
                        ["{NombreCompleto}"] = datos.NombreCompleto ?? "",
                        ["{Documento}"] = datos.Documento ?? "",
                        ["{Observacion}"] = datos.Observacion ?? "",
                        ["{ImporteTotal}"] = datos.ImporteTotal.ToString("N2"),
                        ["{NombreUsuario}"] = datos.NombreUsuario ?? "",
                        ["{NombrePrograma}"] = datos.NombrePrograma ?? "",
                        ["{Manzana}"] = datos.Manzana ?? "",
                        ["{Lote}"] = datos.Lote ?? "",
                        ["{TipoCambio}"] = datos.TipoCambio.ToString("N3"),
                        ["{Moneda}"] = datos.Moneda ?? "",
                        ["{Correlativo}"] = datos.Correlativo ?? ""
                    };

                    foreach (var kv in replacements)
                    {
                        Replace(doc, kv.Key, kv.Value);
                    }

                    // Guardar en stream
                    doc.SaveAs(ms);
                }
                return ms.ToArray();
            }
        }
        private void Replace(DocX doc, string search, string replacement)
        {
            // Soporte para saltos de línea (\n o \r\n)
            if (!string.IsNullOrEmpty(replacement) &&
                (replacement.Contains("\n") || replacement.Contains("\r")))
            {
                var parts = replacement.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                replacement = string.Join(Environment.NewLine, parts);
            }
            var opts = new StringReplaceTextOptions
            {
                SearchValue = search,
                NewValue = replacement ?? string.Empty,
                EscapeRegEx = true,
                RemoveEmptyParagraph = false,
                TrackChanges = false
            };

            doc.ReplaceText(opts);
        }
    }
}
