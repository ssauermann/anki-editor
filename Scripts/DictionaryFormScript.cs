using System;
using AnkiEditor.Query;

namespace AnkiEditor.Scripts
{
    class DictionaryFormScript : Script2
    {
        private readonly IQuery _query;

        public DictionaryFormScript(NoteField self, NoteField other, IQuery query) : base(self, other)
        {
            _query = query;
        }

        public override async void Execute(object sender, EventArgs args)
        {
            if (Other.FieldText == string.Empty || Self.FieldText != string.Empty) return;
            var dic = await _query.DictionaryForm(Other.FieldText);
            Self.FieldText = dic == Other.FieldText ? "" : dic;
        }

    }
}
