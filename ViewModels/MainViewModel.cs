using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace AnkiEditor.ViewModels
{
    public class MainViewModel : Screen
    {
        private string _currentDeckFile;
        private DeckViewModel _currentDeck;
        private bool _deckHasChanged;

        public bool CanOpenDeck => CurrentDeck==null;

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
            }
        }

        public bool CanCloseDeck => CurrentDeck != null;

        public void CloseDeck()
        {
            CurrentDeck = null;
        }

        public bool DeckHasChanged
        {
            get => _deckHasChanged;
            private set
            {
                _deckHasChanged = value;
                NotifyOfPropertyChange(() => DeckHasChanged);
                NotifyOfPropertyChange(() => CanSaveDeck);
            }
        }

        public bool CanSaveDeck => CurrentDeck != null && DeckHasChanged;

        public void SaveDeck()
        {
            //TODO
            throw new NotImplementedException();
            DeckHasChanged = false;
        }

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

    }
}
