using System;
using System.Threading.Tasks;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class DictionaryFormScript : Script
    {
        private readonly IQuery _query;

        public DictionaryFormScript(IQuery query, string displayName) : base(displayName)
        {
            _query = query;
        }

        public override async Task<string> Execute(string src)
        {
            var dic = await _query.DictionaryForm(src);
            return dic == src ? null : dic;
        }
    }
}
