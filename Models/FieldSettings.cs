using Caliburn.Micro;

namespace AnkiEditor.Models
{
    public class FieldSettings : PropertyChangedBase
    {
        private string _language;

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                NotifyOfPropertyChange(() => Language);
            }
        }
    }
}
