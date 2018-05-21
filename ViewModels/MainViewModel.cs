using System.Globalization;
using System.IO;
using System.Linq;
using AnkiEditor.Models;
using AnkiEditor.Settings;
using Caliburn.Micro;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace AnkiEditor.ViewModels
{
    public class MainViewModel : Screen
    {
        #region Fields

        private string _currentDeckFile;

        #endregion

        #region Backing Fields

        private DeckViewModel _currentDeck;

        #endregion

        #region Properties

        public DeckViewModel CurrentDeck
        {
            get => _currentDeck;
            set
            {
                _currentDeck = value;
                NotifyOfPropertyChange(() => CurrentDeck);
                NotifyOfPropertyChange(() => CanOpenDeck);
                NotifyOfPropertyChange(() => CanCloseDeck);
                NotifyOfPropertyChange(() => CanSaveDeck);
            }
        }

        public string Version => "beta 0.1";


        #endregion

        #region Commands

        public bool CanOpenDeck => CurrentDeck == null;

        public void OpenDeck()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (fileDialog.ShowDialog() == true)
            {
                _currentDeckFile = fileDialog.FileName;

                var jsonString = File.ReadAllText(_currentDeckFile);
                var deck = JsonConvert.DeserializeObject<Deck>(jsonString);
                CurrentDeck = new DeckViewModel(deck);

                CurrentDeck.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "DeckHasChanged")
                    {
                        NotifyOfPropertyChange(() => CanSaveDeck);
                    }
                };

                if (File.Exists(_currentDeckFile + ".settings"))
                {
                    var settingsString = File.ReadAllText(_currentDeckFile + ".settings");
                    var set = JsonConvert.DeserializeObject<DeckSettingsModel>(settingsString, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    });
                    foreach (var note in set.LeechedNotes)
                    {
                        CurrentDeck.DeckSettings.LeechedNotes.Add(note);
                    }

                    foreach (var kv in set.FieldSettings)
                    {
                        var f = kv.Value;
                        var fs = CurrentDeck.DeckSettings.FieldSettings[kv.Key];

                        fs.Keep = f.Keep;
                        fs.Language = new CultureInfo(f.Language);
                        fs.Script = CurrentDeck.Scripts.First(x => x.DisplayName == f.Script);
                        fs.ShowPreview = f.ShowPreview;
                        fs.ScriptOverwrite = f.ScriptOverwrite;
                        fs.ScriptSrc = f.ScriptSrc;
                    }
                }
            }
        }

        public bool CanCloseDeck => CurrentDeck != null;

        public void CloseDeck()
        {
            CurrentDeck = null;
            // TODO: This does not reset everything?
            // Reopen another deck and stuff is broken
        }

        public bool CanSaveDeck => CurrentDeck != null && CurrentDeck.DeckHasChanged;

        public void SaveDeck()
        {
            var deck = CurrentDeck.Save();

            var jsonString = JsonConvert.SerializeObject(deck, Formatting.Indented);

            var settings = JsonConvert.SerializeObject(CurrentDeck.DeckSettings.Model(), Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            });

            File.WriteAllText(_currentDeckFile, jsonString);
            File.WriteAllText(_currentDeckFile + ".settings", settings);

            CurrentDeck.DeckHasChanged = false;
        }
        #endregion

    }
}
