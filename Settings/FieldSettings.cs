using System.Globalization;
using AnkiEditor.Scripts;
using AnkiEditor.ViewModels;
using Caliburn.Micro;

namespace AnkiEditor.Settings
{
    public class FieldSettings : PropertyChangedBase
    {
        private CultureInfo _language;
        private bool? _keep;
        private FieldViewModel _scriptSrc;
        private bool _overwrite;
        private Script _script;

        public CultureInfo Language
        {
            get => _language;
            set
            {
                _language = value;
                NotifyOfPropertyChange(() => Language);
            }
        }

        // three state boolean: do keep (true), do not keep (false), keep once (null)
        public bool? Keep
        {
            get => _keep;
            set
            {
                _keep = value;
                NotifyOfPropertyChange(() => Keep);
            }
        }

        public Script Script
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
