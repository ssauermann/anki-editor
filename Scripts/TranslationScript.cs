using System.Threading.Tasks;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class TranslationScript : Script
    {
        private readonly Wadoku _query;

        public TranslationScript(Wadoku query, string displayName) : base(displayName)
        {
            _query = query;
        }

        public override async Task<string> Execute(string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return null;
            var dic = await _query.Translate(src);
            return dic == src ? null : dic;
        }
    }
}
