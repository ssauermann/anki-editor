using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using RestSharp;

namespace AnkiEditor.Query
{
    public class Nihongodera : IQuery
    {
        private const string QueryError = "<Query failed>";

        public async Task<string> DictionaryForm(string input)
        {
            var doc = await QuerySearch(input);
            var dictionary = doc.DocumentNode.SelectSingleNode("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" result-tile__entry \")]");
            dictionary = dictionary?.SelectSingleNode("./span");
            return dictionary?.InnerHtml.Trim() ?? "";
        }

        public async Task<string> FuriganaForm(string input)
        {
            var doc = await QueryConvert(input);

            var furigana = doc?.DocumentNode.SelectSingleNode("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" tool__results \")]")?.InnerHtml.Trim();

            if (furigana == null) return QueryError;

            furigana = furigana?.Replace("<rp>(</rp>", "[").Replace("<rp>)</rp>", "]").Replace("<ruby>", " ").Replace("<rp>", "")
                .Replace("</rp>", "").Replace("<rt>", "").Replace("</rt>", "").Replace("<rb>", "").Replace("</rb>", "").Replace("</ruby>", "");
            return furigana.Trim();
        }

        public async Task<IEnumerable<string>> WordInfo(string input)
        {
            var doc = await QuerySearch(input);

            var badgesFirst = doc.DocumentNode.SelectSingleNode("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" result-tile__pos-badges \")]");
            var badges = badgesFirst?.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" badge \")]");

            if (badges == null) return new List<string>();

            var notes = new List<string>();
            foreach (var badge in badges)
            {
                var text = switchBadge(badge.InnerHtml);
                if (text != string.Empty)
                    notes.Add(text);
            }

            return notes;
        }


        private string switchBadge(string badge)
        {
            switch (badge)
            {
                //Unused: noun, adj-no, exp, aux-v
                case "adj-na":
                    return "な Adjektiv";
                case "adj-i":
                    return "い Adjektiv";
                case "v5u":
                case "v5k":
                case "v5s":
                case "v5t":
                case "v5n":
                case "v5m":
                case "v5r":
                    return "Gruppe I Verb";
                case "v1":
                    return "Gruppe II Verb";
                case "vk":
                case "vs-i":
                case "vs":
                    return "Gruppe III Verb";
                case "adv":
                    return "Adverb";
                case "vi":
                    return "intransitiv";
            }

            return "";

        }

        private Task<HtmlDocument> QuerySearch(string input)
        {
            var url = "https://nihongodera.com/search?input=" + input;
            var web = new HtmlWeb();
            return web.LoadFromWebAsync(url);

        }

        private async Task<HtmlDocument> QueryConvert(string input)
        {
            var text = HttpUtility.UrlEncode(input);

            //TODO Replace with Limelight server
            var client = new RestClient("https://nihongodera.com/tools/convert");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "options%5Bfurigana%5D%5Bkana%5D=hiragana&type=furigana&text=" + text, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteTaskAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);

            return doc;
        }
    }
}
