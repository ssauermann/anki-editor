using System;
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
    public class NoteViewModel : PropertyChangedBase
    {
        private readonly Note _note;
        private readonly Models.NoteModel _noteModel;
        private FieldViewModel _selectedField;

        /// <summary>
        /// Create a new note
        /// </summary>
        /// <param name="noteModel"></param>
        public NoteViewModel(Models.NoteModel noteModel)
        {
            _note = new Note()
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

            Initialize();
        }

        /// <summary>
        /// Load existing note
        /// </summary>
        /// <param name="note"></param>
        /// <param name="noteModel"></param>
        public NoteViewModel(Models.Note note, Models.NoteModel noteModel)
        {
            _note = note;
            _noteModel = noteModel;

            Initialize();
        }

        private void Initialize()
        {

            _note.tags.ForEach(Tags.Add);

            foreach (var fieldViewModel in _noteModel.flds.Zip(_note.fields,
                (model, val) => new FieldViewModel(model.name)
                {
                    Value = val
                }))
            {
                Fields.Add(fieldViewModel);
            }

            foreach (var field in Fields)
            {
                field.PropertyChanged += Field_PropertyChanged;
            }
        }

        private void Field_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => SortName);
        }
        
        public string SortName => Fields[_noteModel.sortf].Value;

        public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>();

        public ObservableCollection<FieldViewModel> Fields { get; } = new ObservableCollection<FieldViewModel>();

        public FieldViewModel SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                NotifyOfPropertyChange(() => SelectedField);
            }
        }
    }
}
