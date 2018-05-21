using System.Threading.Tasks;

namespace AnkiEditor.Scripts
{
    class MirrorScript:Script
    {
        public override Task<string> Execute(string src)
        {
            return Task.FromResult(src);
        }

        public MirrorScript(string displayName) : base(displayName)
        {
        }
    }
}
