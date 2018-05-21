using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using AnkiEditor.Query;
using AnkiEditor.Scripts;
using AnkiEditor.Settings;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class DeckViewModel : PropertyChangedBase
    {
        #region Fields

        private readonly Models.Deck _deckModel;

        private readonly Dictionary<string, Models.NoteModel> _noteModels = new Dictionary<string, Models.NoteModel>();
        private readonly IQuery _query = new Nihongodera();

        #endregion

        #region Constructors

        public DeckViewModel(Models.Deck deckModel)
        {
            _deckModel = deckModel;

            // Load available input languages
            foreach (var lang in InputLanguageManager.Current.AvailableInputLanguages ?? new List<object>())
            {
                Languages.Add(CultureInfo.CreateSpecificCulture(lang.ToString()));
            }

            // Load system default language
            var defaultLang = InputLanguageManager.Current.CurrentInputLanguage;

            // TODO: Remove hard-coded initialization
            // Initialize scripts
            var defaultScript = new NoneScript("None");
            Scripts.Add(defaultScript);
            Scripts.Add(new MirrorScript("Clone"));
            Scripts.Add(new FuriganaScript(_query, "Furigana"));
            Scripts.Add(new DictionaryFormScript(_query, "Dictionary Form"));
            Scripts.Add(new NotesScript(_query, "Notes"));


            // Initialize deck settings
            DeckSettings = new DeckSettings(defaultLang, defaultScript);

            foreach (var noteModel in deckModel.note_models)
            {
                DeckSettings.AddFieldSettings(noteModel);
            }

            // Create dictionary of noteModels
            foreach (var noteModel in deckModel.note_models)
            {
                _noteModels.Add(noteModel.crowdanki_uuid, noteModel);
                NoteModels.Add(noteModel);
            }

            SelectedNoteModel = NoteModels.First();

            foreach (var note in deckModel.notes)
            {
                // Add notes where note model exists
                if (_noteModels.TryGetValue(note.note_model_uuid, out var noteModel))
                {
                    NoteViewModels.Add(new NoteViewModel(note, noteModel, this));
                }
                else
                {
                    // TODO Error handling
                }
            }

            Sort();
        }


        #endregion

        #region Backing Fields

        private NoteViewModel _selectedNoteViewModel;
        private Models.NoteModel _selectedNoteModel;
        private bool _scrollToSelected;
        private bool _deckHasChanged;

        #endregion

        #region Properties

        public DeckSettings DeckSettings { get; set; }

        public string Name => _deckModel.name;
        public string Desc => _deckModel.desc;
        public int NoteCount => NoteViewModels.Count;
        public bool ScrollToSelected
        {
            get => _scrollToSelected;
            set
            {
                _scrollToSelected = value;
                NotifyOfPropertyChange(() => ScrollToSelected);
            }
        }
        public bool DeckHasChanged
        {
            get => _deckHasChanged;
            set
            {
                _deckHasChanged = value;
                NotifyOfPropertyChange(() => DeckHasChanged);
                NotifyOfPropertyChange(() => CanReleech);
                NotifyOfPropertyChange(() => CanUnleech);
            }
        }
        public SmartCollection<NoteViewModel> NoteViewModels { get; } = new SmartCollection<NoteViewModel>();

        public NoteViewModel SelectedNoteViewModel
        {
            get => _selectedNoteViewModel;
            set
            {
                if (value == _selectedNoteViewModel)
                    return;

                _selectedNoteViewModel = value;
                SelectedField = _selectedNoteViewModel?.SelectedField?.Name;
                NotifyOfPropertyChange(() => SelectedNoteViewModel);
                NotifyOfPropertyChange(() => CanDeleteNote);
                NotifyOfPropertyChange(() => SettingsForField);

            }
        }

        public ObservableCollection<Models.NoteModel> NoteModels { get; } = new ObservableCollection<Models.NoteModel>();

        public Models.NoteModel SelectedNoteModel
        {
            get => _selectedNoteModel;
            set
            {
                _selectedNoteModel = value;
                NotifyOfPropertyChange(() => SelectedNoteModel);
            }
        }
        #endregion

        #region Commands

        public bool CanAddNote => SelectedNoteModel != null;

        public void AddNote()
        {
            var newNote = new NoteViewModel(SelectedNoteModel, this);
            NoteViewModels.Add(newNote);
            ScrollToSelected = true;
            SelectedNoteViewModel = newNote;
            DeckHasChanged = true;
            NotifyOfPropertyChange(() => NoteCount);
        }


        public bool CanDeleteNote => SelectedNoteViewModel != null;

        public void DeleteNote()
        {
            NoteViewModels.Remove(SelectedNoteViewModel);
            DeckHasChanged = true;
            NotifyOfPropertyChange(() => NoteCount);
        }

        public bool CanUnleech => NoteViewModels.Count(x => x.Tags.Contains("leech")) > 0 ;

        public void Unleech()
        {
            foreach (var noteViewModel in NoteViewModels)
            {
                if (noteViewModel.Tags.Remove("leech"))
                {
                    DeckSettings.LeechedNotes.Add(noteViewModel.Guid);
                    DeckHasChanged = true;
                }
            }
        }

        public bool CanReleech => DeckSettings.LeechedNotes.Count > 0;

        public void Releech()
        {
            foreach (var note in DeckSettings.LeechedNotes)
            {
                var nm = NoteViewModels.First(n => n.Guid == note);
                nm.AddTag("leech");
            }

            DeckSettings.LeechedNotes.Clear();
        }

        #endregion

        #region FieldSettings


        public ObservableCollection<CultureInfo> Languages { get; } = new ObservableCollection<CultureInfo>();

        public ObservableCollection<Script> Scripts { get; } = new ObservableCollection<Script>();

        private string _selectedField;

        public string SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                NotifyOfPropertyChange(() => SelectedField);
                NotifyOfPropertyChange(() => SettingsForField);
            }
        }

        public FieldSettings SettingsForField => DeckSettings.GetFieldSettings(SelectedNoteViewModel?.Uuid, SelectedField);


        #endregion

        #region Methods

        public void Sort()
        {
            var selection = SelectedNoteViewModel;
            NoteViewModels.Sort(x => x.SortName);
            SelectedNoteViewModel = selection;
        }

        public Models.Deck Save()
        {
            _deckModel.notes.Clear();
            _deckModel.notes.AddRange(from nvm in NoteViewModels select nvm.Save());
            return _deckModel;
        }

        #endregion

    }
}
