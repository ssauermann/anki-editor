using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class FieldViewModel : PropertyChangedBase
    {
        #region Fields
        private readonly NoteViewModel _note;
        #endregion

        #region Constructors

        public FieldViewModel(string fieldName, NoteViewModel note)
        {
            _note = note;
            FieldName = fieldName;

            _note.Deck.DeckSettings.GetFieldSettings(_note.Uuid, FieldName).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "ShowPreview")
                    NotifyOfPropertyChange(() => ShowPreview);
            };
        }

        #endregion

        #region Backing Fields

        private string _value;
        private CultureInfo _inputLanguage;
        private string _selectedText;

        #endregion

        #region Properties

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }
        public string FieldName { get; }

        public CultureInfo InputLanguage
        {
            get => _inputLanguage;
            set
            {
                _inputLanguage = value;
                NotifyOfPropertyChange(() => InputLanguage);
            }
        }

        public bool ShowPreview => _note.Deck.DeckSettings.GetFieldSettings(_note.Uuid, FieldName).ShowPreview;

        public string SelectedText
        {
            get => _selectedText;
            set
            {
                if (_selectedText != value)
                {
                    _selectedText = value;
                    NotifyOfPropertyChange(() => SelectedText);
                }
            }
        }

        #endregion


        public void SelectionChanged(TextBox sender)
        {
            SelectedText = sender.SelectedText;
        }

        public async void ExecuteScript()
        {
            var settings = _note.Deck.DeckSettings.GetFieldSettings(_note.Uuid, FieldName);

            // No script source => do nothing
            if (settings.ScriptSrc == null) return;

            // Value is not empty and should not be overwritten => do nothing
            if (!settings.ScriptOverwrite && !string.IsNullOrWhiteSpace(Value)) return;

            // Execute script asynchronously
            var result = await settings.Script.Execute(_note.Fields.First(x => x.FieldName == settings.ScriptSrc).Value);

            // Null as result == error => do not change current value
            if (result != null) Value = result;
        }

        public void GotFocus()
        {
            _note.SelectedField = this;
        }

        public void ExecuteScripts()
        {
            _note.ExecuteScripts(FieldName);
        }


        #region Object Methods

        public override bool Equals(object obj)
        {
            return obj is FieldViewModel model && Equals(model);
        }

        protected bool Equals(FieldViewModel other)
        {
            return string.Equals(FieldName, other.FieldName);
        }

        public override int GetHashCode()
        {
            return (FieldName != null ? FieldName.GetHashCode() : 0);
        }

        #endregion

    }
}
