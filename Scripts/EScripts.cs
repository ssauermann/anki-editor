using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Scripts
{
    public enum EScripts
    {
        [Description("<None>")]
        None,
        Furigana,
        Notes,
        [Description("Dictionary Form")]
        DictionaryForm,
    }
}
