using AnkiEditor.Scripts;
using AnkiEditor.ViewModels;
using Caliburn.Micro;

namespace AnkiEditor.Models
{
    public class FieldSettings : PropertyChangedBase
    {
        private string _language;
        private bool? _keep;
        private FieldViewModel _scriptSrc;
        private bool _overwrite;
        private Script _script;

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                NotifyOfPropertyChange(() => Language);
            }
        }

        public bool? Keep
        {
            get => _keep;
            set
            {
                _keep = value;
                NotifyOfPropertyChange(() => Keep);
            }
        }

        public Scripts.Script Script
        {
            get => _script;
            set
            {
                _script = value;
                NotifyOfPropertyChange(() => Script);
            }
        }

        public FieldViewModel ScriptSrc
        {
            get => _scriptSrc;
            set
            {
                _scriptSrc = value;
                NotifyOfPropertyChange(() => ScriptSrc);
            }
        }

        public bool ScriptOverwrite
        {
            get => _overwrite;
            set
            {
                _overwrite = value;
                NotifyOfPropertyChange(() => ScriptOverwrite);
            }
        }
    }
}
