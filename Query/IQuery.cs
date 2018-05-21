using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnkiEditor.Query
{
    public interface IQuery
    {
        Task<string> DictionaryForm(string input);
        Task<string> FuriganaForm(string input);
        Task<IEnumerable<string>> WordInfo(string input);
    }
}
