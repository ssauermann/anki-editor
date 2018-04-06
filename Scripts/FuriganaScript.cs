using System;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class FuriganaScript : Script2
    {
        private readonly IQuery _query;

        public FuriganaScript(NoteField self, NoteField other, IQuery query) : base(self, other)
        {
            _query = query;
        }

        public override async void Execute(object sender, EventArgs args)
        {
            if (Other.FieldText == string.Empty || Self.FieldText != string.Empty) return;
            Self.FieldText = await _query.FuriganaForm(Other.FieldText);
        }


    }
}
