using System.Threading.Tasks;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class FuriganaScript : Script
    {
        private readonly Nihongodera _query;

        public FuriganaScript(Nihongodera query, string displayName) : base(displayName)
        {
            _query = query;
        }

        public override Task<string> Execute(string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return Task.FromResult<string>(null);
            return _query.FuriganaForm(src);
        }


    }
}
