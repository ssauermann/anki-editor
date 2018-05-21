using System.Globalization;
using AnkiEditor.Scripts;
using Caliburn.Micro;

namespace AnkiEditor.Settings
{
    public class FieldSettings : PropertyChangedBase
    {
        private CultureInfo _language;
        private bool? _keep;
        private string _scriptSrc;
        private bool _overwrite;
        private Script _script;
        private bool _showPreview;

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

        public string ScriptSrc
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

        public bool ShowPreview
        {
            get => _showPreview;
            set
            {
                _showPreview = value;
                NotifyOfPropertyChange(() => ShowPreview);
            }
        }

        public FieldSettingsModel Model => new FieldSettingsModel()
        {
            Language = Language.Name,
            ScriptSrc = ScriptSrc,
            Script = Script.DisplayName,
            Keep = Keep,
            ScriptOverwrite = ScriptOverwrite,
            ShowPreview = ShowPreview,
        };

    }

    public struct FieldSettingsModel
    {
        public string Language;
        public string ScriptSrc;
        public string Script;
        public bool? Keep;
        public bool ScriptOverwrite;
        public bool ShowPreview;
    }
}
