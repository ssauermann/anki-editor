using System.Collections.Generic;
using System.Windows;
using HtmlAgilityPack;

namespace AnkiEditor
{
    class NotesScript : Script
    {

        public NotesScript(NoteField field, NoteField src) : base(field)
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

            var badgesFirst = doc.DocumentNode.SelectSingleNode("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" result-tile__pos-badges \")]");
            var badges = badgesFirst?.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" badge \")]");

            if (badges == null) return "";

            var notes = new List<string>();
            foreach (var badge in badges)
            {
                notes.Add(switchBadge(badge.InnerHtml));
            }
            notes.RemoveAll(x => x == string.Empty);

            return string.Join(", ", notes);
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
    }
}
