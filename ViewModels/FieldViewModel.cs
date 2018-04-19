using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class FieldViewModel: PropertyChangedBase
    {
        private readonly NoteViewModel _note;

        public FieldViewModel(string name, NoteViewModel note)
        {
            _note = note;
            Name = name;
        }

        private string _value;
        private string _selectedLanguage;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        public string Name { get; }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                NotifyOfPropertyChange(() => SelectedLanguage);
            }
        }

        public void TextChanged(RichTextBox sender)
        {

        }

        public async void ExecuteScript()
        {
            var settings = _note.Deck.FieldSettings[$"{_note.Uuid}_{Name}"];

            if (settings.ScriptSrc == null) return;

            if (settings.ScriptOverwrite || string.IsNullOrWhiteSpace(Value))
            {
                var result = await settings.Script.Execute(settings.ScriptSrc.Value);
                if(result!=null)
                    Value = result;
            }
        }
    }
}
