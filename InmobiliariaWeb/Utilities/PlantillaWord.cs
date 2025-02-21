using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlAgilityPack;

namespace InmobiliariaWeb.Utilities
{
    public class PlantillaWord
    {
        private readonly string _templatePath;

        public PlantillaWord(string templatePath)
        {
            _templatePath = templatePath;
        }

        public byte[] GenerateWordDocument(string htmlContent)
        {
            // Crear una copia temporal de la plantilla
            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".docx");
            File.Copy(_templatePath, tempFilePath, true);

            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(tempFilePath, true))
                {
                    // Mantener las secciones existentes que contienen la configuración de página
                    MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                    Document doc = mainPart.Document;
                    Body body = doc.Body;

                    // Convertir el HTML
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlContent);

                    // Procesar cada nodo HTML y agregar al documento
                    foreach (var node in htmlDoc.DocumentNode.ChildNodes)
                    {
                        if (node.NodeType != HtmlNodeType.Text || !string.IsNullOrWhiteSpace(node.InnerText))
                        {
                            ProcessNode(node, body);
                        }
                    }

                    // Guardar los cambios
                    doc.Save();
                }

                // Leer el archivo generado
                return File.ReadAllBytes(tempFilePath);
            }
            finally
            {
                // Limpiar el archivo temporal
                if (File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch { /* Ignorar errores de limpieza */ }
                }
            }
        }

        private void ProcessNode(HtmlNode node, Body body)
        {
            if (node.NodeType == HtmlNodeType.Text)
            {
                if (!string.IsNullOrWhiteSpace(node.InnerText))
                {
                    var paragraph = new Paragraph(
                        new Run(
                            new Text { Text = node.InnerText.Trim(), Space = SpaceProcessingModeValues.Preserve }
                        )
                    );
                    body.AppendChild(paragraph);
                }
                return;
            }

            switch (node.Name.ToLower())
            {
                case "p":
                    var p = new Paragraph();
                    p.AppendChild(new ParagraphProperties(
                        new SpacingBetweenLines { After = "0", Line = "360", LineRule = LineSpacingRuleValues.Auto }
                    ));
                    ProcessChildNodes(node, p);
                    body.AppendChild(p);
                    break;

                case "div":
                    ProcessChildNodes(node, body);
                    break;
            }
        }

        private void ProcessChildNodes(HtmlNode parentNode, OpenXmlElement parent)
        {
            foreach (var node in parentNode.ChildNodes)
            {
                if (parent is Body body)
                {
                    ProcessNode(node, body);
                    continue;
                }

                if (parent is Paragraph paragraph)
                {
                    switch (node.Name.ToLower())
                    {
                        case "#text":
                            if (!string.IsNullOrWhiteSpace(node.InnerText))
                            {
                                paragraph.AppendChild(
                                    new Run(
                                        new Text { Text = node.InnerText.Trim(), Space = SpaceProcessingModeValues.Preserve }
                                    )
                                );
                            }
                            break;

                        case "b":
                        case "strong":
                            paragraph.AppendChild(
                                new Run(
                                    new RunProperties(new Bold()),
                                    new Text { Text = node.InnerText.Trim(), Space = SpaceProcessingModeValues.Preserve }
                                )
                            );
                            break;

                        case "u":
                            paragraph.AppendChild(
                                new Run(
                                    new RunProperties(new Underline { Val = UnderlineValues.Single }),
                                    new Text { Text = node.InnerText.Trim(), Space = SpaceProcessingModeValues.Preserve }
                                )
                            );
                            break;

                        case "sup":
                            paragraph.AppendChild(
                                new Run(
                                    new RunProperties(new VerticalTextAlignment { Val = VerticalPositionValues.Superscript }),
                                    new Text { Text = node.InnerText.Trim(), Space = SpaceProcessingModeValues.Preserve }
                                )
                            );
                            break;

                        case "br":
                            paragraph.AppendChild(new Run(new Break()));
                            break;
                    }
                }
            }
        }
    }
}
