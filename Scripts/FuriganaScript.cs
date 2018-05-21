using System.Threading.Tasks;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class FuriganaScript : Script
    {
        private readonly IQuery _query;

        public FuriganaScript(IQuery query, string displayName) : base(displayName)
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
