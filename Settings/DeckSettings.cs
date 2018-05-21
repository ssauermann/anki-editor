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
        public readonly Dictionary<string, FieldSettings> FieldSettings = new Dictionary<string, FieldSettings>();

        public HashSet<string> LeechedNotes { get; } = new HashSet<string>();

        public DeckSettingsModel Model()
        {
            var model = new DeckSettingsModel()
            {
                LeechedNotes = LeechedNotes,
                FieldSettings = new Dictionary<string, FieldSettingsModel>(),
            };

            foreach (var kv in FieldSettings)
            {
                model.FieldSettings.Add(kv.Key, kv.Value.Model);
            }

            return model;
        }

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
                FieldSettings.Add($"{prefix}_{fld.name}", new FieldSettings()
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
            return FieldSettings.GetValueOrDefault($"{noteModelUuid}_{fieldName}");
        }

        public IEnumerable<string> GetAllFieldsWithSetting(string noteModelUuid, Func<FieldSettings, bool> condition)
        {
            return from kv in FieldSettings where kv.Key.StartsWith(noteModelUuid + "_") && condition(kv.Value) select kv.Key.Substring(noteModelUuid.Length + 1);
        }
    }

    public struct DeckSettingsModel
    {
        public HashSet<string> LeechedNotes;
        public Dictionary<string, FieldSettingsModel> FieldSettings;
    }
}
