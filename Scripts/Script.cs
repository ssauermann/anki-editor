namespace AnkiEditor.Scripts
{
    public abstract class Script
    {
        protected readonly NoteField Self;

        protected Script(NoteField self)
        {
            Self = self;
        }

        public abstract void Start();

        public abstract void Stop();
    }
}
