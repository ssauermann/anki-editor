using System;
using System.Threading.Tasks;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class NotesScript : Script
    {
        private readonly IQuery _query;

        public NotesScript(IQuery query, string displayName) : base(displayName)
        {
            _query = query;
        }

        public override async Task<string> Execute(string src)
        {
            var info = await _query.WordInfo(src);
            return string.Join(", ", info);
        }
    }
}
