using System;

namespace AnkiEditor.Scripts
{
    public abstract class Script2 : Script
    {
        protected readonly NoteField Other;

        protected Script2(NoteField self, NoteField other) : base(self)
        {
            Other = other;
        }

        public override void Start()
        {
            Other.TextLostFocusAdd(Execute);
        }

        public override void Stop()
        {
            Other.TextLostFocusRemove(Execute);
        }

        public abstract void Execute(object sender, EventArgs args);
    }
}
