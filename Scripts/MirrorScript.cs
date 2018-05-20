using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
