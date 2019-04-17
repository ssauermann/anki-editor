using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
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
            if (dictionaryForm != "")
            {
                queryString = dictionaryForm;
            }

            var doc = await QuerySearch(queryString);
            var results = doc.DocumentNode.SelectNodes(XPathForClass("resultline"));

            foreach (var r in results)
            {
                var japanese = r.SelectSingleNode("./td[2]/div[1]/a/span");
                if (japanese?.InnerText.Trim() == queryString)
                {
                    var senses = r.SelectNodes("./td[3]/div[2]/section/section[2]/span");
                    var result = new List<List<string>>();
                    result.Add(new List<string>());
                    foreach (var sense in senses)
                    {
                        foreach (var c in sense.ChildNodes)
                        {
                            if (c.HasClass("rel"))
                            {
                                continue;
                            }
                            else if (c.HasClass("indexnr"))
                            {
                                if (c.InnerText != "1")
                                {
                                    result.Add(new List<string>());
                                }
                            }
                            else if (c.HasClass("token"))
                            {
                                var word = c.FirstChild.InnerText.Trim().Replace("&nbsp;", "");
                                if (word != "")
                                {
                                    result.Last().Add(word);
                                }
                            }
                            else if (c.NodeType == HtmlNodeType.Text)
                            {
                                var word = c.InnerText.Trim().Replace("&nbsp;", "");
                                if (word.Last() == '.')
                                {
                                    word = word.Substring(0, word.Length - 1);
                                }

                                if (word != "")
                                {
                                    result.Last().Add(word);
                                }
                            }
                        }
                    }

                    var resultString = "";

                    foreach (var seq in result)
                    {
                        foreach (var w in seq)
                        {
                            resultString += w + ", ";
                        }

                        resultString = resultString.Substring(0, resultString.Length - 2);
                        resultString += "; ";
                    }
                    resultString = resultString.Substring(0, resultString.Length - 2);

                    return resultString;
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
