﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AnkiEditor.Models;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class NoteViewModel : PropertyChangedBase
    {
        private readonly Note _note;
        private readonly Models.NoteModel _noteModel;
        public DeckViewModel Deck { get; }
        private FieldViewModel _selectedField;

        /// <summary>
        /// Create a new note
        /// </summary>
        /// <param name="noteModel"></param>
        /// <param name="deck"></param>
        public NoteViewModel(Models.NoteModel noteModel, DeckViewModel deck)
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

        private void Initialize()
        {
            Uuid = _noteModel.crowdanki_uuid;
            _note.tags.ForEach(Tags.Add);

            foreach (var fieldViewModel in _noteModel.flds.Zip(_note.fields,
                (model, val) =>
                {
                    var settings = Deck.GetFieldSettings(_noteModel.crowdanki_uuid, model.name);
                    var defaultLang = settings.Language;

                    var fvm = new FieldViewModel(model.name, this)
                    {
                        Value = val,
                        InputLanguage = defaultLang,
                    };

                    settings.PropertyChanged += (sender, args) => fvm.InputLanguage = settings.Language;
                    settings.ScriptSrc = fvm;

                    return fvm;
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
            var changed = sender as FieldViewModel;

            if (e.PropertyName == nameof(FieldViewModel.Value))
            {
                Deck.DeckHasChanged = true;


                if (Equals(changed, SortField))
                {
                    NotifyOfPropertyChange(() => SortName);
                    Deck.NoteViewModelsSorted.Refresh();
                    //TODO Enable scrolling when DeckView code behind is updated on ScrollToSelected change instead of selection changed
                    //Deck.ScrollToSelected = true; 
                }
            }

        }

        public string SortName => SortField.Value;

        public FieldViewModel SortField => Fields[_noteModel.sortf];

        public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>();

        public ObservableCollection<FieldViewModel> Fields { get; } = new ObservableCollection<FieldViewModel>();

        public FieldViewModel SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                NotifyOfPropertyChange(() => SelectedField);
                Deck.SelectedField = value?.Name;
            }
        }

        public string Uuid { get; private set; }
    }
}