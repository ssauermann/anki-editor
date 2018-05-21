using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using MarkupConverter;
using Xceed.Wpf.Toolkit;

namespace AnkiEditor
{
    class HtmlFormatter : ITextFormatter
    {
        public string GetText(FlowDocument document)
        {
            TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Xaml);
                var xaml = Encoding.UTF8.GetString(ms.ToArray());
                return HtmlFromXamlConverter.ConvertXamlToHtml(xaml, false);
            }
        }

        public void SetText(FlowDocument document, string text)
        {
            var xaml = HtmlToXamlConverter.ConvertHtmlToXaml(text, false);

            try
            {
                if (String.IsNullOrEmpty(xaml))
                {
                    document.Blocks.Clear();
                }
                else
                {
                    TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
                    {
                        tr.Load(ms, DataFormats.Xaml);
                    }
                }
            }
            catch
            {
                throw new InvalidDataException("Data provided is not in the correct Xaml format.");
            }

            
        }
    }
}
