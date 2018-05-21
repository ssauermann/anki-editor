using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiEditor.Settings;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class NoteViewModel : PropertyChangedBase
    {
        #region Fields

        private readonly Models.Note _note;
        private readonly Models.NoteModel _noteModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new note
        /// </summary>
        /// <param name="noteModel"></param>
        /// <param name="deck"></param>
        public NoteViewModel(Models.NoteModel noteModel, DeckViewModel deck)
        {
            _note = new Models.Note()
            {
                __type__ = "Note",
                data = string.Empty,
                fields = new List<string>(new string[noteModel.flds.Count]),
                flags = 0,
                guid = Utils.Guid64(),
                note_model_uuid = noteModel.crowdanki_uuid,
                tags = new List<string>()
            };
            _noteModel = noteModel;
            Deck = deck;

            Initialize();
        }

        /// <summary>
        /// Load existing note
        /// </summary>
        /// <param name="note"></param>
        /// <param name="noteModel"></param>
        /// <param name="deck"></param>
        public NoteViewModel(Models.Note note, Models.NoteModel noteModel, DeckViewModel deck)
        {
            _note = note;
            _noteModel = noteModel;
            Deck = deck;
            Initialize();
        }

        #endregion

        #region Backing Fields

        private FieldViewModel _selectedField;
        private string _selectedTag;
        private string _newTag = "";

        #endregion

        #region Properties
        public DeckViewModel Deck { get; }

        public string Uuid { get; private set; }

        public string Guid { get; private set; }

        public string SortName => SortField.Value;

        public FieldViewModel SortField => Fields[_noteModel.sortf];

        public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>();

        public ObservableCollection<FieldViewModel> Fields { get; } = new ObservableCollection<FieldViewModel>();

        public ObservableCollection<string> FieldsStrings { get; } = new ObservableCollection<string>();

        public FieldViewModel SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                NotifyOfPropertyChange(() => SelectedField);
                Deck.SelectedField = value?.FieldName;
            }
        }

        public string SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
                NotifyOfPropertyChange(() => SelectedTag);
                NotifyOfPropertyChange(() => CanDeleteTag);
            }
        }

        public string NewTag
        {
            get => _newTag;
            set
            {
                _newTag = value;
                NotifyOfPropertyChange(() => NewTag);
                NotifyOfPropertyChange(() => CanAddTag);
            }
        }
        
        #endregion

        #region Methods

        private void Initialize()
        {
            Uuid = _noteModel.crowdanki_uuid;
            Guid = _note.guid;

            // Extract tags
            _note.tags.ForEach(Tags.Add);

            // Zip field name and field value to get a list with (name, value) tuple entries.
            foreach (var fieldViewModel in _noteModel.flds.Zip(_note.fields,
                (model, val) =>
                {
                    // Create field view model and setup the field settings
                    var settings = Deck.DeckSettings.GetFieldSettings(_noteModel.crowdanki_uuid, model.name);
                    var defaultLang = settings.Language;

                    var fvm = new FieldViewModel(model.name, this)
                    {
                        Value = val,
                        InputLanguage = defaultLang,
                    };

                    settings.ScriptSrc = fvm.FieldName;
                    settings.PropertyChanged += (sender, args) =>
                    {
                        if(args.PropertyName == nameof(FieldSettings.Language))
                            fvm.InputLanguage = settings.Language;
                    };

                    return fvm;
                }))
            {
                Fields.Add(fieldViewModel);
                FieldsStrings.Add(fieldViewModel.FieldName);
            }

            // Register property changed event for all fields
            foreach (var field in Fields)
            {
                field.PropertyChanged += Field_PropertyChanged;
            }
        }


        public Models.Note Save()
        {
            _note.tags.Clear();
            _note.tags.AddRange(Tags);
            _note.fields.Clear();
            _note.fields.AddRange(from field in Fields select "" + field.Value);
            return _note;
        }

        public bool CanAddTag => NewTag != "";

        public void AddTag()
        {
            AddTag(NewTag);
            NewTag = "";
        }

        public void AddTag(string newTag)
        {
            if (!Tags.Contains(newTag))
            {
                Tags.Add(newTag);
                Deck.DeckHasChanged = true;
            }
        }

        public void DeleteTag()
        {
            Tags.Remove(SelectedTag);
            Deck.DeckHasChanged = true;
        }

        public bool CanDeleteTag => SelectedTag != null;


        public void ExecuteScripts(string triggerField)
        {
            var fields = Deck.DeckSettings.GetAllFieldsWithSetting(_noteModel.crowdanki_uuid, x => x.ScriptSrc == triggerField);
            foreach (var field in fields)
            {
                Fields.First(x => x.FieldName == field).ExecuteScript();
            }
        }

        #endregion

        #region Event Handlers
        private void Field_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var changed = sender as FieldViewModel;

            if (e.PropertyName == nameof(FieldViewModel.Value))
            {
                Deck.DeckHasChanged = true;


                if (Equals(changed, SortField))
                {
                    NotifyOfPropertyChange(() => SortName);
                    Deck.Sort();
                    //TODO Enable scrolling when DeckView code behind is updated on ScrollToSelected change instead of selection changed
                    //Deck.ScrollToSelected = true; 
                }
            }

        }

        #endregion

    }
}