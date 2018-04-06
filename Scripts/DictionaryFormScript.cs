using System;
using System.Threading.Tasks;
using System.Windows;
using HtmlAgilityPack;

namespace AnkiEditor.Scripts
{
    class DictionaryFormScript : Script2
    {

        public DictionaryFormScript(NoteField self, NoteField other) : base(self, other)
        {
        }

        public override async void Execute(object sender, EventArgs args)
        {
            if (Other.FieldText == string.Empty || Self.FieldText != string.Empty) return;
            var dic = await NihongoderaQuery(Other.FieldText);
            Self.FieldText = dic == Other.FieldText ? "" : dic;
        }


        private async Task<string> NihongoderaQuery(string kana)
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
