using System.Globalization;
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

        public FieldViewModel(string name, NoteViewModel note)
        {
            _note = note;
            Name = name;
        }

        #endregion

        #region Backing Fields

        private string _value;
        private CultureInfo _inputLanguage;

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
        public string Name { get; }

        public CultureInfo InputLanguage
        {
            get => _inputLanguage;
            set
            {
                _inputLanguage = value;
                NotifyOfPropertyChange(() => InputLanguage);
            }
        }
        #endregion


        public void TextChanged(RichTextBox sender)
        {

        }

        public async void ExecuteScript()
        {
            var settings = _note.Deck.DeckSettings.GetFieldSettings(_note.Uuid, Name);

            // No script source => do nothing
            if (settings.ScriptSrc == null) return;

            // Value is not empty and should not be overwritten => do nothing
            if (!settings.ScriptOverwrite && !string.IsNullOrWhiteSpace(Value)) return;

            // Execute script asynchronously
            var result = await settings.Script.Execute(settings.ScriptSrc.Value);

            // Null as result == error => do not change current value
            if (result != null) Value = result;
        }


        #region Object Methods

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

        #endregion

    }
}
