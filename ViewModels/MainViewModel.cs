using System;
using System.IO;
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
                var deck = JsonConvert.DeserializeObject<Models.Deck>(jsonString);
                CurrentDeck = new DeckViewModel(deck);

                CurrentDeck.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "DeckHasChanged")
                    {
                        NotifyOfPropertyChange(() => CanSaveDeck);
                    }
                };
            }
        }

        public bool CanCloseDeck => CurrentDeck != null;

        public void CloseDeck()
        {
            CurrentDeck = null;
        }

        public bool CanSaveDeck => CurrentDeck != null && CurrentDeck.DeckHasChanged;

        public void SaveDeck()
        {
            //TODO
            throw new NotImplementedException();
            CurrentDeck.DeckHasChanged = false;
        }

        #endregion

    }
}
