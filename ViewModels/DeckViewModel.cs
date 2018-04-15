using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnkiEditor.Models;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class DeckViewModel : PropertyChangedBase
    {
        private readonly Deck _deckModel;
        private readonly Dictionary<string, Models.NoteModel> _noteModels = new Dictionary<string, Models.NoteModel>();

        public DeckViewModel(Models.Deck deckModel)
        {
            _deckModel = deckModel;

            // Create dictionary of noteModels
            foreach (var noteModel in deckModel.note_models)
            {
                _noteModels.Add(noteModel.crowdanki_uuid, noteModel);
            }
            
            foreach (var note in deckModel.notes)
            {
                // Add notes where notemodel exists
                if (_noteModels.TryGetValue(note.note_model_uuid, out var noteModel))
                {
                    NoteViewModels.Add(new NoteViewModel(note, noteModel));
                }
            }
        }

        public string Name => _deckModel.name;
        public string Desc => _deckModel.desc;

        public int NoteCount => NoteViewModels.Count;


        public bool CanAddNote => true;

        public void AddNote()
        {
            //TODO
            throw new NotImplementedException();
        }

        public bool CanDeleteNote => SelectedNoteViewModel != null;

        public void DeleteNote()
        {
            //TODO
            throw new NotImplementedException();
        }


        public ObservableCollection<NoteViewModel> NoteViewModels { get; } = new ObservableCollection<NoteViewModel>();
        private NoteViewModel _selectedNoteViewModel;

        public NoteViewModel SelectedNoteViewModel
        {
            get => _selectedNoteViewModel;
            set
            {
                _selectedNoteViewModel = value;
                NotifyOfPropertyChange(() => SelectedNoteViewModel);
                NotifyOfPropertyChange(() => CanDeleteNote);

            }
        }
    }
}
