using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private CultureInfo _inputLanguage;

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

        public CultureInfo InputLanguage
        {
            get => _inputLanguage;
            set
            {
                _inputLanguage = value;
                NotifyOfPropertyChange(() => InputLanguage);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is FieldViewModel model && Equals(model);
        }

        protected bool Equals(FieldViewModel other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
