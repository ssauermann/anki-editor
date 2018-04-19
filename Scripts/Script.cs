using System;
using System.Threading.Tasks;
using AnkiEditor.Models;
using AnkiEditor.ViewModels;

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
