using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AnkiEditor.Scripts;
using Caliburn.Micro;

namespace AnkiEditor.Settings
{
    public class DeckSettings
    {
        private readonly CultureInfo _defaultLanguage;
        private readonly Script _defaultScript;
        private readonly Dictionary<string, FieldSettings> _fieldSettings = new Dictionary<string, FieldSettings>();

        public DeckSettings(CultureInfo defaultLanguage, Script defaultScript)
        {
            _defaultLanguage = defaultLanguage;
            _defaultScript = defaultScript;
        }

        public void AddFieldSettings(Models.NoteModel noteModel)
        {
            var prefix = noteModel.crowdanki_uuid;

            foreach (var fld in noteModel.flds)
            {
                _fieldSettings.Add($"{prefix}_{fld.name}", new FieldSettings()
                {
                    Language = _defaultLanguage,
                    Keep = false,
                    ScriptOverwrite = false,
                    Script = _defaultScript,
                });
            }
        }

        public FieldSettings GetFieldSettings(string noteModelUuid, string fieldName)
        {
            return _fieldSettings.GetValueOrDefault($"{noteModelUuid}_{fieldName}");
        }

        public IEnumerable<string> GetAllFieldsWithSetting(string noteModelUuid, Func<FieldSettings, bool> condition)
        {
            return from kv in _fieldSettings where kv.Key.StartsWith(noteModelUuid + "_") && condition(kv.Value) select kv.Key.Substring(noteModelUuid.Length + 1);
        }
    }
}
