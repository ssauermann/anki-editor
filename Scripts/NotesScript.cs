using System;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class NotesScript : Script2
    {
        private readonly IQuery _query;

        public NotesScript(NoteField self, NoteField other, IQuery query) : base(self, other)
        {
            _query = query;
        }

        public override async void Execute(object sender, EventArgs args)
        {
            if (Other.FieldText == string.Empty || Self.FieldText != string.Empty) return;
            var info = await _query.WordInfo(Other.FieldText);
            Self.FieldText = string.Join(", ", info);
        }


    }
}
