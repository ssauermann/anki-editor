using HtmlAgilityPack;
using System.Threading.Tasks;

namespace AnkiEditor.Query
{
    public class Wadoku : IQuery
    {
        private readonly Nihongodera _query;

        public Wadoku(Nihongodera query)
        {
            _query = query;
        }

        private const string QueryError = "<Query failed>";

        private string XPathForClass(string clazz) => $"//*[contains(concat(\" \", normalize-space(@class), \" \"), \" {clazz} \")]";

        public async Task<string> Translate(string input)
        {
            var dictionaryForm = await _query.DictionaryForm(input);

            var queryString = input;
            if(dictionaryForm != "")
            {
                queryString = dictionaryForm;
            }

            var doc = await QuerySearch(queryString);
            var results = doc.DocumentNode.SelectNodes(XPathForClass("resultline"));

            foreach (var r in results)
            {
                var japanese = r.SelectSingleNode("./td[2]/div[1]/a/span");
                if (japanese?.InnerHtml.Trim() == queryString)
                {
                    var senses = r.SelectNodes("./td[3]/div[2]/section/section[2]/span");
                    var result = "";
                    foreach (var sense in senses)
                    {
                        var german = sense.InnerText.Split(';'); // .SelectNodes("./[parent::span[@class='sense']]");


                        for(int i=1; i<german.Length;i++)
                        {
                            result += german[i].Trim();
                            result += ", ";
                        }
                        result = result.Substring(0, result.Length - 3);

                        result += "; ";
                    }

                    return result.Substring(0, result.Length - 2); ;
                }
            }

            return "";
        }


        private Task<HtmlDocument> QuerySearch(string input)
        {
            var url = "https://www.wadoku.de/search/" + input;
            var web = new HtmlWeb();
            return web.LoadFromWebAsync(url);

        }
    }
}
