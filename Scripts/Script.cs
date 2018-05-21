using System.Threading.Tasks;

namespace AnkiEditor.Scripts
{
    public abstract class Script
    {
        public string DisplayName { get;}

        protected Script(string displayName)
        {
            DisplayName = displayName;
        }

        public abstract Task<string> Execute(string src);
    }
}
