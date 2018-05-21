using System.Threading.Tasks;

namespace AnkiEditor.Scripts
{
    public class NoneScript: Script
    {
        public override Task<string> Execute(string src)
        {
            // Do nothing
            return Task.FromResult<string>(null);
        }

        public NoneScript(string displayName) : base(displayName)
        {
        }
    }
}
