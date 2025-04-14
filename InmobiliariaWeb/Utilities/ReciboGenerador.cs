using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace InmobiliariaWeb.Utilities
{
    public class ReciboGenerador
    {
        private readonly string _templatePath;
        public ReciboGenerador(string webRootPath)
        {
            // Ruta completa a la plantilla
            _templatePath = Path.Combine(webRootPath, "template", "FormatoRecibo.docx");
        }
        public async Task<byte[]> GenerarReciboAsync(ReciboBE datos)
        {
            // Copiar la plantilla en memoria
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(_templatePath, FileMode.Open, FileAccess.Read))
                {
                    await fs.CopyToAsync(ms);
                }

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    ReplacePlaceholder(body, "{NumeroRecibo}", datos.NumeroRecibo);
                    ReplacePlaceholder(body, "{FechaPago}", datos.FechaPago.ToString("dd/MM/yyyy"));
                    ReplacePlaceholder(body, "{NombreCompleto}", datos.NombreCompleto);
                    ReplacePlaceholder(body, "{Documento}", datos.Documento);
                    ReplacePlaceholder(body, "{Observacion}", datos.Observacion);
                    ReplacePlaceholder(body, "{ImporteTotal}", datos.ImporteTotal.ToString("N2"));
                    ReplacePlaceholder(body, "{NombreUsuario}", datos.NombreUsuario);
                    ReplacePlaceholder(body, "{NombrePrograma}", datos.NombrePrograma);
                    ReplacePlaceholder(body, "{TipoCambio}", datos.TipoCambio.ToString("N3"));

                    wordDoc.MainDocumentPart.Document.Save();
                }

                return ms.ToArray();
            }
        }

        private void ReplacePlaceholder(Body body, string placeholder, string value)
        {
            var paragraphs = body.Descendants<Paragraph>();

            foreach (var paragraph in paragraphs)
            {
                var texts = paragraph.Descendants<Text>().ToList();
                var combinedText = string.Concat(texts.Select(t => t.Text));

                if (combinedText.Contains(placeholder))
                {
                    combinedText = combinedText.Replace(placeholder, value);

                    // Limpiar nodos antiguos
                    foreach (var t in texts)
                    {
                        t.Text = string.Empty;
                    }

                    // Poner el texto reemplazado en el primer nodo
                    if (texts.Count > 0)
                    {
                        texts[0].Text = combinedText;
                    }
                }
            }
        }

    }
}
