using System;
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
            return _query.FuriganaForm(src);
        }


    }
}
