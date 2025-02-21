using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using HtmlAgilityPack;

namespace InmobiliariaWeb.Utilities
{
    public class HtmlToWordConverter
    {
        public string TemplatePath { get; set; }

        public HtmlToWordConverter(string templatePath = null)
        {
            TemplatePath = templatePath;
        }

        public void ConvertHtmlToWord(string html, string outputPath)
        {
            // Si existe una plantilla, la copiamos como base
            if (!string.IsNullOrEmpty(TemplatePath) && File.Exists(TemplatePath))
            {
                File.Copy(TemplatePath, outputPath, true);
            }

            // Crear nuevo documento o abrir la plantilla
            using (WordprocessingDocument wordDoc =
                string.IsNullOrEmpty(TemplatePath)
                    ? WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document)
                    : WordprocessingDocument.Open(outputPath, true))
            {
                // Si es un documento nuevo, configurar las partes necesarias
                if (string.IsNullOrEmpty(TemplatePath))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    mainPart.Document.Body = new Body();
                }

                // Cargar el HTML usando HtmlAgilityPack
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Procesar cada nodo
                foreach (var node in htmlDoc.DocumentNode.ChildNodes)
                {
                    ProcessNode(node, wordDoc.MainDocumentPart.Document.Body);
                }

                wordDoc.MainDocumentPart.Document.Save();
            }
        }

        private void ProcessNode(HtmlNode node, Body body)
        {
            if (node.NodeType == HtmlNodeType.Text)
            {
                var run = new Run(new Text(node.InnerText));
                var paragraph = new Paragraph(run);
                body.AppendChild(paragraph);
                return;
            }

            var newParagraph = new Paragraph();

            switch (node.Name.ToLower())
            {
                case "p":
                    ProcessParagraphContent(node, newParagraph);
                    body.AppendChild(newParagraph);
                    break;

                case "b":
                case "strong":
                    var boldRun = new Run(
                        new RunProperties(new Bold()),
                        new Text(node.InnerText));
                    newParagraph.AppendChild(boldRun);
                    body.AppendChild(newParagraph);
                    break;

                case "u":
                    var underlineRun = new Run(
                        new RunProperties(new Underline() { Val = UnderlineValues.Single }),
                        new Text(node.InnerText));
                    newParagraph.AppendChild(underlineRun);
                    body.AppendChild(newParagraph);
                    break;

                case "sup":
                    var superscriptRun = new Run(
                        new RunProperties(new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript }),
                        new Text(node.InnerText));
                    newParagraph.AppendChild(superscriptRun);
                    body.AppendChild(newParagraph);
                    break;
            }
        }

        private void ProcessParagraphContent(HtmlNode paragraphNode, Paragraph paragraph)
        {
            foreach (var childNode in paragraphNode.ChildNodes)
            {
                if (childNode.NodeType == HtmlNodeType.Text)
                {
                    paragraph.AppendChild(new Run(new Text(childNode.InnerText)));
                    continue;
                }

                switch (childNode.Name.ToLower())
                {
                    case "b":
                    case "strong":
                        paragraph.AppendChild(new Run(
                            new RunProperties(new Bold()),
                            new Text(childNode.InnerText)));
                        break;

                    case "u":
                        paragraph.AppendChild(new Run(
                            new RunProperties(new Underline() { Val = UnderlineValues.Single }),
                            new Text(childNode.InnerText)));
                        break;

                    case "sup":
                        paragraph.AppendChild(new Run(
                            new RunProperties(new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript }),
                            new Text(childNode.InnerText)));
                        break;
                }
            }
        }
    }
}
