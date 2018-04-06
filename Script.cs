namespace AnkiEditor
{
    public abstract class Script
    {
        public string Name = "<None>";
        protected readonly NoteField Self;

        protected Script(NoteField field)
        {
            Self = field;
        }

        public abstract void Start();

        public abstract void Stop();

        public override string ToString() => Name;
    }
}
