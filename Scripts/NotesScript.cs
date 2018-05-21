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
            if (string.IsNullOrWhiteSpace(src)) return null;
            var info = await _query.WordInfo(src);
            return string.Join(", ", info);
        }
    }
}
