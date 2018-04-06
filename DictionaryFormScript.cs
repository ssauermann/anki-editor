using System.Windows;
using HtmlAgilityPack;

namespace AnkiEditor
{
    class DictionaryFormScript : Script
    {

        public DictionaryFormScript(NoteField field, NoteField src) : base(field)
        {
            this.Src = src;
        }

        public NoteField Src { get; set; }

        public override void Start()
        {
            Src.TextLostFocus += OnLostFocus;
        }

        private void OnLostFocus(object sender, RoutedEventArgs args)
        {
            if (Src.FieldText == string.Empty || Self.FieldText != string.Empty) return;
            var dic = NihongoderaQuery(Src.FieldText);
            Self.FieldText = dic == Src.FieldText ? "" : dic;
        }

        public override void Stop()
        {
            Self.TextLostFocus -= OnLostFocus;
        }



        private string NihongoderaQuery(string kana)
        {
            var url = "https://nihongodera.com/search?input=" + kana;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var dictionary = doc.DocumentNode.SelectSingleNode("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" result-tile__entry \")]");
            dictionary = dictionary?.SelectSingleNode("./span");
            return dictionary?.InnerHtml.Trim() ?? "";
        }
    }
}
