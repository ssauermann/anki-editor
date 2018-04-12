using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Caliburn.Micro;
using HtmlAgilityPack;
using MarkupConverter;

namespace AnkiEditor.ViewModels
{
    public class EditorViewModel : PropertyChangedBase
    {
        private string _rawText = "FI";

        public string RawText
        {
            get => _rawText;
            set
            {
                _rawText = value;
                NotifyOfPropertyChange(() => RawText);
            }
        }

        private bool _isTextSelected;

        public bool IsTextSelected
        {
            get => _isTextSelected;
            set
            {
                _isTextSelected = value;
                NotifyOfPropertyChange(() => IsTextSelected);
                NotifyOfPropertyChange(() => CanMakeItalic);
                NotifyOfPropertyChange(() => CanMakeBold);
                NotifyOfPropertyChange(() => CanMakeUnderline);
                NotifyOfPropertyChange(() => CanCreateFurigana);
            }
        }

        public bool CanMakeItalic => IsTextSelected;
        public bool CanMakeBold => IsTextSelected;
        public bool CanMakeUnderline => IsTextSelected;
        public bool CanCreateFurigana => IsTextSelected;

        public void CreateFurigana()
        {
        }

        public void TextChanged(RichTextBox sender)
        {
            RawText = RtbToHtml(sender);
        }

        private static string RtbToHtml(RichTextBox rtb)
        {
            string xamlString;

            // Read content from rtb as xaml
            using (var rtfMemoryStream = new MemoryStream())
            {
                var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range.Save(rtfMemoryStream, DataFormats.Xaml);
                rtfMemoryStream.Seek(0, SeekOrigin.Begin);
                using (var rtfStreamReader = new StreamReader(rtfMemoryStream))
                {
                    xamlString = rtfStreamReader.ReadToEnd();
                }
            }

            // Add required flow document tags
            xamlString = "<FlowDocument>" + xamlString + "</FlowDocument>";

            // Convert from xaml to html
            var htmlString = HtmlFromXamlConverter.ConvertXamlToHtml(xamlString, false);

            // Remove outer div element to get rid of font settings, text alignments, etc.
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            htmlString = doc.DocumentNode.SelectSingleNode("//div").InnerHtml;

            return htmlString;
        }
    }
}