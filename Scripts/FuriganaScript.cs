using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using HtmlAgilityPack;
using RestSharp;

namespace AnkiEditor.Scripts
{
    class FuriganaScript : Script2
    {

        public FuriganaScript(NoteField self, NoteField other) : base(self, other)
        {
        }

        public override async void Execute(object sender, EventArgs args)
        {
            if (Other.FieldText == string.Empty || Self.FieldText != string.Empty) return;
            Self.FieldText = await NihongoderaQuery(Other.FieldText);
        }

        private async Task<string> NihongoderaQuery(string kana)
        {
            var text = HttpUtility.UrlEncode(kana);

            //TODO Replace with Limelight server
            var client = new RestClient("https://nihongodera.com/tools/convert");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "options%5Bfurigana%5D%5Bkana%5D=hiragana&type=furigana&text=" + text, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteTaskAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return kana;

            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);

            var furigana = doc.DocumentNode.SelectSingleNode("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" tool__results \")]")
                ?.InnerHtml.Trim();
            furigana = furigana ?? kana;
            furigana = furigana.Replace("<rp>(</rp>", "[").Replace("<rp>)</rp>", "]").Replace("<ruby>", " ").Replace("<rp>", "")
                .Replace("</rp>", "").Replace("<rt>", "").Replace("</rt>", "").Replace("<rb>", "").Replace("</rb>", "").Replace("</ruby>", "");
            return furigana.Trim();
        }

    }
}
