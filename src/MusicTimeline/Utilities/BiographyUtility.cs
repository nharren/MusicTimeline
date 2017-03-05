using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class BiographyUtility
    {
        public static FlowDocument LoadDocument(string flowDocumentString)
        {
            if (flowDocumentString == null)
            {
                return null;
            }

            var parserContext = new ParserContext();
            parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            parserContext.XmlSpace = "preserve";

            var flowDocumentBytes = Encoding.UTF8.GetBytes(flowDocumentString);

            using (var flowDocumentMemoryStream = new MemoryStream(flowDocumentBytes))
            {
                var flowDocument = (FlowDocument)XamlReader.Load(flowDocumentMemoryStream, parserContext);
                flowDocument.LineHeight = 24.0;
                flowDocument.FontSize = 15.3333333333;
                flowDocument.FontFamily = new FontFamily("Segoe UI");
                flowDocument.PagePadding = new Thickness(0.0, 5.0, 0.0, 0.0);
                flowDocument.Foreground = Brushes.Black;
                flowDocument.IsHyphenationEnabled = true;

                return flowDocument;
            }
        }

        public static string CleanXaml(string xaml)
        {
            if (xaml == null)
            {
                return xaml;
            }

            xaml = xaml.Trim();

            RemoveEmptyParagraphs(ref xaml);
            RemoveNamespace(ref xaml);
            RemoveSection("Media", ref xaml);
            RemoveSection("References", ref xaml);
            RemoveSection("Bibliography", ref xaml);
            RemoveSection("See also", ref xaml);
            RemoveSection("External links", ref xaml);
            RemoveSection("Free scores", ref xaml);
            RemoveSection("Scores", ref xaml);
            RemoveSection("Music scores", ref xaml);
            RemoveSection("Notes", ref xaml);
            RemoveSection("Further reading", ref xaml);
            RemoveSection("Cited sources", ref xaml);
            RemoveSection("Other sources", ref xaml);

            return xaml;
        }

        private static void RemoveEmptyParagraphs(ref string xaml)
        {
            xaml = xaml.Replace("<Paragraph />", "");
        }

        private static void RemoveNamespace(ref string xaml)
        {
            xaml = xaml.Replace(@" xml:space=""preserve"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""", "");
        }

        private static void RemoveSection(string title, ref string xaml)
        {
            xaml = Regex.Replace(xaml, $@"(<Paragraph[^>]*?><Run[^>]*?>{title}<\/Run>.*?)(?=<Paragraph[^>]+?|<\/FlowDocument>)", "");
        }
    }
}