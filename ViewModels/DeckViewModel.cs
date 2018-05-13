using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AnkiEditor.Models;
using AnkiEditor.Query;
using AnkiEditor.Scripts;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class DeckViewModel : PropertyChangedBase
    {
        private readonly Deck _deckModel;
        private readonly Dictionary<string, Models.NoteModel> _noteModels = new Dictionary<string, Models.NoteModel>();
        private readonly IQuery _query = new Nihongodera();

        public DeckViewModel(Models.Deck deckModel)
        {
            _deckModel = deckModel;

            foreach (var lang in InputLanguageManager.Current.AvailableInputLanguages ?? new List<object>())
            {
                Languages.Add(CultureInfo.CreateSpecificCulture(lang.ToString()));
            }

            var defaultLang = InputLanguageManager.Current.CurrentInputLanguage;

            var defaultScript = new NoneScript("None");
            Scripts.Add(defaultScript);
            Scripts.Add(new MirrorScript("Clone"));
            Scripts.Add(new FuriganaScript(_query, "Furigana"));
            Scripts.Add(new DictionaryFormScript(_query, "Dictionary Form"));
            Scripts.Add(new NotesScript(_query, "Notes"));


            foreach (var noteModel in deckModel.note_models)
            {
                var prefix = noteModel.crowdanki_uuid;

                foreach (var fld in noteModel.flds)
                {
                    FieldSettings.Add($"{prefix}_{fld.name}", new FieldSettings()
                    {
                        Language = defaultLang,
                        Keep = false,
                        ScriptOverwrite = false,
                        Script = defaultScript,
                    });
                }
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
                // Add notes where notemodel exists
                if (_noteModels.TryGetValue(note.note_model_uuid, out var noteModel))
                {
                    NoteViewModels.Add(new NoteViewModel(note, noteModel, this));
                }
            }

        }

        public string Name => _deckModel.name;
        public string Desc => _deckModel.desc;

        public int NoteCount => NoteViewModels.Count;


        public bool CanAddNote => SelectedNoteModel != null;

        public void AddNote()
        {
            var newNote = new NoteViewModel(SelectedNoteModel, this);
            NoteViewModels.Add(newNote);
            ScrollToSelected = true;
            SelectedNoteViewModel = newNote;
        }

        public bool ScrollToSelected
        {
            get => _scrollToSelected;
            set
            {
                _scrollToSelected = value;
                NotifyOfPropertyChange(() => ScrollToSelected);
            }
        }

        public bool CanDeleteNote => SelectedNoteViewModel != null;

        public void DeleteNote()
        {
            NoteViewModels.Remove(SelectedNoteViewModel);
        }


        public ObservableCollection<NoteViewModel> NoteViewModels { get; } = new ObservableCollection<NoteViewModel>();

        private ICollectionView _sortedNoteViewModel;
        public ICollectionView NoteViewModelsSorted
        {
            get
            {
                if (_sortedNoteViewModel == null)
                {
                    _sortedNoteViewModel = CollectionViewSource.GetDefaultView(NoteViewModels);
                    _sortedNoteViewModel.SortDescriptions.Add(new SortDescription(nameof(NoteViewModel.SortName), ListSortDirection.Ascending));
                }

                return _sortedNoteViewModel;
            }
        }

        private NoteViewModel _selectedNoteViewModel;
        private Models.NoteModel _selectedNoteModel;
        private bool _scrollToSelected;

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

        #region FieldSettings


        public ObservableCollection<CultureInfo> Languages { get; } = new ObservableCollection<CultureInfo>();

        public ObservableCollection<Scripts.Script> Scripts { get; } = new ObservableCollection<Script>();

        public Dictionary<string, FieldSettings> FieldSettings = new Dictionary<string, FieldSettings>();
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

        public FieldSettings GetFieldSettings(string noteModelUuid, string fieldName)
        {
            return FieldSettings.GetValueOrDefault($"{noteModelUuid}_{fieldName}");
        }

        public FieldSettings SettingsForField => GetFieldSettings(SelectedNoteViewModel.Uuid, SelectedField);


        #endregion

    }
}
