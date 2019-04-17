﻿using System.Threading.Tasks;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class DictionaryFormScript : Script
    {
        private readonly Nihongodera _query;

        public DictionaryFormScript(Nihongodera query, string displayName) : base(displayName)
        {
            _query = query;
        }

        public override async Task<string> Execute(string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return null;
            var dic = await _query.DictionaryForm(src);
            return dic == src ? null : dic;
        }
    }
}
