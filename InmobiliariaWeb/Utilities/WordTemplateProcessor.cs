using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace InmobiliariaWeb.Utilities
{
    public class WordTemplateProcessor
    {
        private readonly string _templatePath;

        public WordTemplateProcessor(string templatePath)
        {
            _templatePath = templatePath;
        }

        public async Task<byte[]> GenerateDocument(Dictionary<string, string> replacements)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (!File.Exists(_templatePath))
                {
                    throw new FileNotFoundException($"No se encontró la plantilla en: {_templatePath}");
                }

                using (var templateStream = File.OpenRead(_templatePath))
                {
                    await templateStream.CopyToAsync(ms);
                }
                ms.Position = 0;

                using (WordprocessingDocument doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    // Imprimir todo el contenido del documento para inspección
                    System.Diagnostics.Debug.WriteLine("=== Contenido completo del documento ===");
                    foreach (var paragraph in body.Descendants<Paragraph>())
                    {
                        System.Diagnostics.Debug.WriteLine($"Párrafo: '{paragraph.InnerText}'");
                    }

                    foreach (var paragraph in body.Descendants<Paragraph>())
                    {
                        foreach (var run in paragraph.Descendants<Run>())
                        {
                            foreach (var text in run.Elements<Text>())
                            {
                                string originalText = text.Text;
                                System.Diagnostics.Debug.WriteLine($"Procesando texto: '{originalText}'");

                                // Inspección específica para ParrafoInicial
                                if (originalText.Contains("ParrafoInicial"))
                                {
                                    System.Diagnostics.Debug.WriteLine("=== Análisis detallado de ParrafoInicial ===");
                                    System.Diagnostics.Debug.WriteLine($"Longitud del texto: {originalText.Length}");
                                    System.Diagnostics.Debug.WriteLine($"Caracteres (ASCII): {string.Join(", ", originalText.Select(c => ((int)c).ToString()))}");
                                }

                                string newText = originalText;

                                foreach (var replacement in replacements)
                                {
                                    string placeholder = $"{{{replacement.Key}}}";

                                    // Logging específico para ParrafoInicial
                                    if (replacement.Key == "ParrafoInicial")
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Buscando ParrafoInicial:");
                                        System.Diagnostics.Debug.WriteLine($"Placeholder: '{placeholder}'");
                                        System.Diagnostics.Debug.WriteLine($"Texto actual: '{newText}'");
                                        System.Diagnostics.Debug.WriteLine($"¿Contiene el placeholder?: {newText.Contains(placeholder)}");

                                        // Comparación carácter por carácter
                                        System.Diagnostics.Debug.WriteLine("Comparación carácter por carácter:");
                                        for (int i = 0; i < Math.Min(newText.Length, placeholder.Length); i++)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Pos {i}: '{newText[i]}' ({(int)newText[i]}) vs '{placeholder[i]}' ({(int)placeholder[i]})");
                                        }
                                    }

                                    if (newText.Contains(placeholder))
                                    {
                                        newText = newText.Replace(placeholder, replacement.Value ?? string.Empty);
                                        System.Diagnostics.Debug.WriteLine($"Reemplazo realizado para {replacement.Key}. Nuevo texto: '{newText}'");
                                    }
                                }

                                if (originalText != newText)
                                {
                                    text.Text = newText;
                                }
                            }
                        }
                    }

                    doc.MainDocumentPart.Document.Save();
                }

                return ms.ToArray();
            }
        }
    }
}
