using System.Collections.Generic;

namespace AnkiEditor
{
    class NoteModel
    {
        public NoteModel(string uuid)
        {
            this.uuid = uuid;
        }
        public readonly List<string> fields = new List<string>();
        public readonly string uuid;
        public int NoteCount = 0;
    }
}
